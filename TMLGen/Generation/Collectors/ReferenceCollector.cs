using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Xml.Linq;
using System.Xml.XPath;
using TMLGen.Generation.Helpers;

namespace TMLGen.Generation.Collectors
{
    public struct ReferenceFlagPaths
    {
        public HashSet<string> flagPaths;
        public HashSet<string> tagPaths;
        public HashSet<string> scriptFlagPaths;
        public HashSet<string> questFlagPaths;
    }

    public class ReferenceCollector
    {
        private readonly JsonNode rootNode;
        private readonly Dictionary<Guid, HashSet<string>> flagDict = [];
        private readonly Dictionary<Guid, string> linesDict = [];
        private readonly XDocument localizationDoc;
        private readonly string outputPath;
        private readonly ReferenceFlagPaths referenceFlagPaths;
        private readonly JsonSerializerOptions options;

        private static readonly List<string> validNodeTypes = ["TagAnswer", "TagGreeting", "TagCinematic"];
        private static readonly Dictionary<string, string> nodeTitles = new()
        {
            {"TagAnswer", "Answer"},
            {"TagGreeting", "Greeting"},
            {"TagCinematic", "Cinematic"},
        };

        /// <summary>
        /// Represents the _ref.json file.
        /// </summary>
        /// <param name="flagDict"></param>
        /// <param name="linesDict"></param>
        private class ReferenceFile(Dictionary<Guid, HashSet<string>> flagDict, Dictionary<Guid, string> linesDict)
        {
            public Dictionary<Guid, HashSet<string>> Flags { get; } = flagDict;
            public Dictionary<Guid, string> Lines { get; } = linesDict;
        }

        public ReferenceCollector(string dialogDoc, string outputPath, string localizationDoc, ReferenceFlagPaths referenceFlagPaths, JsonSerializerOptions options)
        {
            rootNode = JsonNode.Parse(new StreamReader(dialogDoc).BaseStream);
            this.localizationDoc = XDocument.Load(localizationDoc);
            this.outputPath = GetReferenceName(outputPath);
            this.referenceFlagPaths = referenceFlagPaths;
            this.options = options;
        }

        private string GetReferenceName(string outputPath)
        {
            string directoryName = Path.GetDirectoryName(outputPath);
            string fileName = Path.GetFileNameWithoutExtension(outputPath);
            return Path.Join(directoryName, fileName + "_ref.json");
        }

        public void Collect()
        {
            JsonArray nodeArray = rootNode["save"]!["regions"]!["dialog"]!["nodes"]![0]!["node"].AsArray();
            
            foreach (JsonNode node in nodeArray)
            {
                string nodeType = node["constructor"]!["value"].ToString();
                if (!validNodeTypes.Contains(nodeType)) continue;

                Guid nodeId = Guid.Parse(node["UUID"]!["value"].ToString());
                HashSet<string> flags = GetNodeFlags(node);

                if (nodeType == "TagCinematic")
                {
                    HandleTagCinematic(nodeId, node, flags);
                }
                else
                {
                    JsonNode taggedTextsCollection = node["TaggedTexts"]![0]!["TaggedText"];
                    if (taggedTextsCollection != null)
                    {
                        foreach (JsonNode textNode in taggedTextsCollection.AsArray())
                        {
                            GetTags(textNode, flags);
                            BindHandlesAndFlags(textNode, nodeId, flags);
                        }
                        if (!linesDict.ContainsKey(nodeId))
                        {
                            linesDict.Add(nodeId, nodeTitles[nodeType]);
                        }
                    }
                }
            }
            WriteOutput();
        }

        private void WriteOutput()
        {
            ReferenceFile refFile = new(flagDict, linesDict);
            string refJson = JsonSerializer.Serialize(refFile, options);
            File.WriteAllText(outputPath, refJson);
        }

        private void HandleTagCinematic(Guid nodeId, JsonNode node, HashSet<string> flags)
        {
            string name = "Cinematic";
            JsonArray dataElements = node["editorData"]![0]!["data"].AsArray();
            foreach (JsonNode data in dataElements)
            {
                if (data["key"]!["value"].ToString() == "logicalname")
                {
                    name = data["val"]!["value"].ToString();
                }
            }
            flagDict.Add(nodeId, flags);
            linesDict.Add(nodeId, name);
        }

        private void BindHandlesAndFlags(JsonNode textNode, Guid nodeId, HashSet<string> flags)
        {
            JsonArray textArray = textNode["TagTexts"]![0]!["TagText"]?.AsArray();
            if (textArray != null)
            {
                foreach (JsonNode textData in textArray)
                {
                    string translatedString = GetTranslatedString(textData["TagText"]!["handle"].ToString());
                    if (textData["CustomSequenceId"] != null)
                    {
                        Guid sequenceId = Guid.Parse(textData["CustomSequenceId"]!["value"].ToString());
                        linesDict.TryAdd(sequenceId, translatedString);
                        flagDict.TryAdd(sequenceId, flags);
                    }
                    else
                    {
                        linesDict.TryAdd(nodeId, translatedString);
                        flagDict.TryAdd(nodeId, flags);
                    }
                }
            }
        }

        private string GetTranslatedString(string handle)
        {
            XElement translationEle = localizationDoc.XPathSelectElement("contentList/content[@contentuid='" + handle + "']");
            return translationEle == null ? string.Empty : translationEle.Value;
        }

        private HashSet<string> GetNodeFlags(JsonNode node)
        {
            HashSet<string> res = [];
            HashSet<string> foundFlags = [];
            JsonNode flagCollection = node["checkflags"]![0]!["flaggroup"];

            if (flagCollection != null)
            {
                foreach (JsonNode flagSet in flagCollection.AsArray())
                {
                    JsonNode flagNode = flagSet["flag"];
                    if (flagNode != null)
                    {
                        foreach (JsonNode flag in flagNode.AsArray())
                        {
                            string id = flag["UUID"]!["value"].ToString();
                            if (!foundFlags.Contains(id))
                            {
                                string name = GetFlagName(id, referenceFlagPaths.flagPaths);
                                if (name == string.Empty) name = GetFlagName(id, referenceFlagPaths.tagPaths);
                                if (name == string.Empty) name = GetScriptFlagName(id);
                                if (name == string.Empty) name = GetQuestFlagName(id);
                                res.Add(name);
                            }
                            foundFlags.Add(id);
                        }
                    }
                }
            }

            return res;
        }

        private string GetQuestFlagName(string flagId)
        {
            foreach (string questFlagFile in referenceFlagPaths.questFlagPaths)
            {
                XDocument flagDoc = XDocument.Load(questFlagFile);
                XElement flagEle = flagDoc.XPathSelectElement("save/region/node/children/node[children[node[attribute[@id='DialogFlagGUID'][@value='" + flagId + "']]]]");
                XElement flagChildEle = flagDoc.XPathSelectElement("save/region/node/children/node/children/node[attribute[@id='DialogFlagGUID'][@value='" + flagId + "']]");
                if (flagEle != null)
                {
                    string questName = flagEle.XPathSelectElement("./attribute[@id='QuestID']").Attribute("value").Value;
                    string stageName = flagChildEle.XPathSelectElement("./attribute[@id='ID']").Attribute("value").Value;
                    return questName + " - " + stageName;
                }
            }
            return string.Empty;
        }

        private string GetScriptFlagName(string flagId)
        {
            foreach (string scriptFlagFile in referenceFlagPaths.scriptFlagPaths)
            {
                XDocument flagDoc = XDocument.Load(scriptFlagFile);
                XElement flagEle = flagDoc.XPathSelectElement("save/region/node/children/node[attribute[@id='UUID'][@value='" + flagId + "']]");
                if (flagEle != null)
                {
                    return flagEle.XPathSelectElement("./attribute[@id='name']").Attribute("value").Value;
                }
            }
            return string.Empty;
        }

        private void GetTags(JsonNode textNode, HashSet<string> flags)
        {
            HashSet<string> foundTags = [];
            JsonNode ruleCollection = textNode["RuleGroup"]![0]!["Rules"]![0]!["Rule"];
            if (ruleCollection != null)
            {
                foreach (JsonNode tagCollection in ruleCollection.AsArray())
                {
                    JsonNode tags = tagCollection["Tags"]?[0]?["Tag"];
                    if (tags != null)
                    {
                        foreach (JsonNode tag in tags.AsArray())
                        {
                            string item = tag["Object"]!["value"].ToString();
                            if (!foundTags.Contains(item))
                            {
                                flags.Add(GetFlagName(item, referenceFlagPaths.tagPaths));
                            }
                            foundTags.Add(item);
                        }
                    }
                }
            }
        }

        private string GetFlagName(string flagId, HashSet<string> pathCollection)
        {
            foreach (string flagDir in pathCollection)
            {
                foreach (string file in Directory.EnumerateFiles(flagDir))
                {
                    if (flagId == Path.GetFileNameWithoutExtension(file))
                    {
                        string flagFile = PreparationHelper.SaveToLsxFile(file);
                        XDocument flagDoc = XDocument.Load(flagFile);
                        XElement flagEle = flagDoc.XPathSelectElement("save/region/node/attribute[@id='Name']");
                        File.Delete(flagFile);
                        return flagEle != null ? flagEle.Attribute("value").Value : string.Empty;
                    }
                }
            }
            return string.Empty;
        }
    }
}
