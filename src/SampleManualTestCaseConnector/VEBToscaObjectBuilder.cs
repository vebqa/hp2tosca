// Decompiled with JetBrains decompiler
// Type: TCMigrationAPI.ToscaObjectBuilder
// Assembly: TCMigrationAPI, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 93C9F9CD-9C35-421C-88D3-D39A2409861E
// Assembly location: C:\Program Files (x86)\TRICENTIS\Tosca Testsuite\TCMigrationAPI.dll

using System;
using System.Collections.Generic;

namespace TCMigrationAPI
{
    public class VEBToscaObjectBuilder
    {
        private bool isModuleOptimizationAllowed = true;
        private Dictionary<string, int> moduleLibrary = new Dictionary<string, int>();
        private readonly string engine;
        private readonly string xModuleBusinessType;
        private readonly ToscaObjectDefinition definitionObject;

        public bool ModuleOptimizationAllowed
        {
            set
            {
                this.isModuleOptimizationAllowed = value;
            }
        }

        public VEBToscaObjectBuilder(ToscaObjectDefinition definition, string engine)
        {
            this.definitionObject = definition;
            if (!(engine == "Html"))
            {
                if (engine == "SAPGUI")
                {
                    this.engine = "SapEngine";
                    this.xModuleBusinessType = "Window";
                }
                else
                {
                    this.engine = "Html";
                    this.xModuleBusinessType = "HtmlDocument";
                }
            }
            else
            {
                this.engine = "Html";
                this.xModuleBusinessType = "HtmlDocument";
            }
        }

        public int CreateFolder(string name, FolderType folderType, int parentId)
        {
            Dictionary<string, string> attributeList = new Dictionary<string, string>();
            attributeList.Add(TCBusinessObjectAttribute.Name, name);
            switch (folderType)
            {
                case FolderType.Modules:
                    attributeList.Add(TCBusinessObjectAttribute.ContentPolicy, "++--------");
                    break;
                case FolderType.TestCases:
                    attributeList.Add(TCBusinessObjectAttribute.ContentPolicy, "+-+-------");
                    break;
                case FolderType.TestCaseDesign:
                    attributeList.Add(TCBusinessObjectAttribute.ContentPolicy, "+-----+----");
                    break;
                case FolderType.Execution:
                    attributeList.Add(TCBusinessObjectAttribute.ContentPolicy, "+------+---");
                    break;
                case FolderType.Requirements:
                    attributeList.Add(TCBusinessObjectAttribute.ContentPolicy, "+---+-----");
                    break;
                case FolderType.Reporting:
                    attributeList.Add(TCBusinessObjectAttribute.ContentPolicy, "+----+-----");
                    break;
                case FolderType.ComponentFolder:
                    attributeList.Add(TCBusinessObjectAttribute.ContentPolicy, "+---------");
                    break;
            }
            return this.definitionObject.AddObject(TCBusinessObject.TCFolder, attributeList, parentId);
        }

        public int CreateXModuleAttribute(string name, string businessType, string defaultActionMode, int parentId, Dictionary<string, string> idParams)
        {
            if (this.isModuleOptimizationAllowed && this.definitionObject.GetChildObjects(parentId).ContainsKey(name))
                return this.definitionObject.GetChildObjects(parentId)[name];
            Dictionary<string, string> attributeList = new Dictionary<string, string>();
            attributeList.Add(TCBusinessObjectAttribute.Name, name);
            attributeList.Add(TCBusinessObjectAttribute.BusinessType, businessType);
            attributeList.Add(TCBusinessObjectAttribute.DefaultActionMode, defaultActionMode);
            int parentId1 = this.definitionObject.AddObject(TCBusinessObject.XModuleAttribute, attributeList, parentId);
            attributeList.Clear();
            attributeList.Add(TCBusinessObjectAttribute.Name, "BusinessAssociation");
            string str1 = businessType;
            BusinessType businessType1 = BusinessType.Column;
            string str2 = businessType1.ToString();
            if (str1 == str2)
            {
                attributeList.Add(TCBusinessObjectAttribute.Value, "Columns");
            }
            else
            {
                string str3 = businessType;
                businessType1 = BusinessType.Row;
                string str4 = businessType1.ToString();
                if (str3 == str4)
                {
                    attributeList.Add(TCBusinessObjectAttribute.Value, "Rows");
                }
                else
                {
                    string str5 = businessType;
                    businessType1 = BusinessType.Cell;
                    string str6 = businessType1.ToString();
                    if (str5 == str6)
                        attributeList.Add(TCBusinessObjectAttribute.Value, "Cells");
                    else
                        attributeList.Add(TCBusinessObjectAttribute.Value, "Descendants");
                }
            }
            attributeList.Add(TCBusinessObjectAttribute.ParamType, ToscaObjectBuilder.ParamType.ConfigurationParam.ToString());
            attributeList.Add(TCBusinessObjectAttribute.Visible, "true");
            this.definitionObject.AddObject(TCBusinessObject.XParam, attributeList, parentId1);
            this.CreateIdParam("Engine", this.engine, ToscaObjectBuilder.ParamType.ConfigurationParam, parentId1);
            if (idParams != null)
            {
                foreach (KeyValuePair<string, string> idParam in idParams)
                    this.CreateIdParam(idParam.Key, idParam.Value, ToscaObjectBuilder.ParamType.TechnicalIdParam, parentId1);
            }
            return parentId1;
        }

        public int CreateSpecialExecutionTaskAttribute(string name, int specialExecutionTaskId)
        {
            if (this.isModuleOptimizationAllowed && this.definitionObject.GetChildObjects(specialExecutionTaskId).ContainsKey(name))
                return this.definitionObject.GetChildObjects(specialExecutionTaskId)[name];
            int parentId = this.definitionObject.AddObject(TCBusinessObject.XModuleAttribute, new Dictionary<string, string>()
      {
        {
          TCBusinessObjectAttribute.Name,
          name
        },
        {
          TCBusinessObjectAttribute.DefaultActionMode,
          ActionMode.Input.ToString()
        }
      }, specialExecutionTaskId);
            this.CreateIdParam("Parameter", "true", 8, parentId);
            return parentId;
        }

        public int CreateXModuleAttributeAsTable(string name, int parentId, Dictionary<string, string> technicalIdParams, string rowHeaders, string columnHeaders)
        {
            if (this.isModuleOptimizationAllowed && this.definitionObject.GetChildObjects(parentId).ContainsKey(name))
                return this.definitionObject.GetChildObjects(parentId)[name];
            if (rowHeaders != "")
                rowHeaders = ";" + rowHeaders;
            if (columnHeaders != "")
                columnHeaders = ";" + columnHeaders;
            int xmoduleAttribute = this.CreateXModuleAttribute(name, "Table", "Select", parentId, technicalIdParams);
            this.CreateIdParam("DecisiveColumns", "*", ToscaObjectBuilder.ParamType.SteeringParam, xmoduleAttribute);
            this.CreateIdParam("HeaderRow", "1", ToscaObjectBuilder.ParamType.SteeringParam, xmoduleAttribute);
            this.CreateIdParam("IgnoreInvisibleTableContent", "True", ToscaObjectBuilder.ParamType.SteeringParam, xmoduleAttribute);
            Dictionary<string, string> attributeList = new Dictionary<string, string>();
            attributeList.Add(TCBusinessObjectAttribute.Name, "<Row>");
            attributeList.Add("Cardinality", "0-N");
            Dictionary<string, string> dictionary1 = attributeList;
            string defaultActionMode1 = TCBusinessObjectAttribute.DefaultActionMode;
            ActionMode actionMode = ActionMode.Select;
            string str1 = actionMode.ToString();
            dictionary1.Add(defaultActionMode1, str1);
            attributeList.Add(TCBusinessObjectAttribute.BusinessType, "Row");
            int parentId1 = this.definitionObject.AddObject(TCBusinessObject.XModuleAttribute, attributeList, xmoduleAttribute);
            this.CreateIdParam("Engine", this.engine, ToscaObjectBuilder.ParamType.ConfigurationParam, parentId1);
            this.CreateIdParam("BusinessAssociation", "Rows", ToscaObjectBuilder.ParamType.ConfigurationParam, parentId1);
            this.CreateIdParam("ExplicitName", "$1;$<n>;$last;$header;$firstEmptyRow;$lastContentRow" + rowHeaders, ToscaObjectBuilder.ParamType.ConfigurationParam, parentId1);
            attributeList.Clear();
            attributeList.Add(TCBusinessObjectAttribute.Name, "<Col>");
            attributeList.Add("Cardinality", "0-N");
            Dictionary<string, string> dictionary2 = attributeList;
            string defaultActionMode2 = TCBusinessObjectAttribute.DefaultActionMode;
            actionMode = ActionMode.Select;
            string str2 = actionMode.ToString();
            dictionary2.Add(defaultActionMode2, str2);
            attributeList.Add(TCBusinessObjectAttribute.BusinessType, "Column");
            int parentId2 = this.definitionObject.AddObject(TCBusinessObject.XModuleAttribute, attributeList, xmoduleAttribute);
            this.CreateIdParam("Engine", this.engine, ToscaObjectBuilder.ParamType.ConfigurationParam, parentId2);
            this.CreateIdParam("BusinessAssociation", "Columns", ToscaObjectBuilder.ParamType.ConfigurationParam, parentId2);
            this.CreateIdParam("ExplicitName", "$1;$<n>;$last" + columnHeaders, ToscaObjectBuilder.ParamType.ConfigurationParam, parentId2);
            attributeList.Clear();
            attributeList.Add(TCBusinessObjectAttribute.Name, "<Cell>");
            attributeList.Add("Cardinality", "0-N");
            Dictionary<string, string> dictionary3 = attributeList;
            string defaultActionMode3 = TCBusinessObjectAttribute.DefaultActionMode;
            actionMode = ActionMode.Verify;
            string str3 = actionMode.ToString();
            dictionary3.Add(defaultActionMode3, str3);
            attributeList.Add(TCBusinessObjectAttribute.BusinessType, "Cell");
            int parentId3 = this.definitionObject.AddObject(TCBusinessObject.XModuleAttribute, attributeList, parentId1);
            this.CreateIdParam("Engine", this.engine, ToscaObjectBuilder.ParamType.ConfigurationParam, parentId3);
            this.CreateIdParam("BusinessAssociation", "Cells", ToscaObjectBuilder.ParamType.ConfigurationParam, parentId3);
            this.CreateIdParam("ExplicitName", "$1;$<n>;$last" + columnHeaders, ToscaObjectBuilder.ParamType.ConfigurationParam, parentId3);
            int parentId4 = this.definitionObject.AddObject(TCBusinessObject.XModuleAttribute, attributeList, parentId2);
            this.CreateIdParam("Engine", this.engine, ToscaObjectBuilder.ParamType.ConfigurationParam, parentId4);
            this.CreateIdParam("BusinessAssociation", "Cells", ToscaObjectBuilder.ParamType.ConfigurationParam, parentId4);
            this.CreateIdParam("ExplicitName", "$1;$<n>;$last;$header;$firstEmptyRow;$lastContentRow" + columnHeaders, ToscaObjectBuilder.ParamType.ConfigurationParam, parentId4);
            return xmoduleAttribute;
        }

        public int CreateTestCase(string name, int parentId)
        {
            return this.definitionObject.AddObject(TCBusinessObject.TestCase, new Dictionary<string, string>()
            {
                {
                    TCBusinessObjectAttribute.Name, name
                }
            }, parentId);
        }

        public int CreateTestCase(string name, string description, int parentId)
        {
            Dictionary<string, string> additionalPropertyList = new Dictionary<string, string>()
            {
                {
                    "Beschreibung", "desc:" + description
                },
                {
                    "Regression", "true"
                }
            };

            return this.definitionObject.AddObject(TCBusinessObject.TestCase, new Dictionary<string, string>()
                {
                    {
                        TCBusinessObjectAttribute.Name, name
                    },
                    {
                        TCBusinessObjectAttribute.Description, description
                    }
                },
                parentId, true, additionalPropertyList);
        }

        public void ConvertTestCaseToTemplate(int testCaseId, int testSheetId)
        {
            Dictionary<string, string> attributeList = new Dictionary<string, string>()
      {
        {
          TCBusinessObjectAttribute.SchemaPath,
          ""
        }
      };
            Dictionary<string, string> additionalPropertyList = new Dictionary<string, string>()
      {
        {
          "SchemaDefinition",
          " -" + (object) testSheetId + " "
        }
      };
            this.definitionObject.AddObject(TCBusinessObject.TestCaseTemplateDetail, attributeList, testCaseId, true, additionalPropertyList);
        }

        public int CreateTestStepFolder(string name, int parentId)
        {
            return this.definitionObject.AddObject(TCBusinessObject.TestStepFolder, new Dictionary<string, string>()
      {
        {
          TCBusinessObjectAttribute.Name,
          name
        }
      }, parentId);
        }

        public int CreateXTestStepFromXModule(string name, int moduleId, int parentId)
        {
            return this.definitionObject.AddObject(TCBusinessObject.XTestStep, new Dictionary<string, string>()
      {
        {
          TCBusinessObjectAttribute.Name,
          name
        },
        {
          "ReorderAllowed",
          "true"
        }
      }, parentId, true, new Dictionary<string, string>()
      {
        {
          "Module",
          " -" + (object) moduleId + " "
        }
      });
        }

        public int SetXTestStepValue(string value, int parentTestStepId, int moduleAttributeId, string actionMode)
        {
            Dictionary<string, string> attributeList = new Dictionary<string, string>();
            if (!value.Contains("="))
            {
                attributeList.Add(TCBusinessObjectAttribute.Value, value);
            }
            else
            {
                string[] strArray = value.Split(new char[1] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                attributeList.Add(TCBusinessObjectAttribute.ActionProperty, strArray[0]);
                attributeList.Add(TCBusinessObjectAttribute.Value, strArray[1]);
            }
            if (actionMode != null)
                attributeList.Add(TCBusinessObjectAttribute.ActionMode, actionMode);
            Dictionary<string, string> additionalPropertyList = new Dictionary<string, string>()
      {
        {
          "ModuleAttribute",
          " -" + (object) moduleAttributeId + " "
        }
      };
            return this.definitionObject.AddObject(TCBusinessObject.XTestStepValue, attributeList, parentTestStepId, true, additionalPropertyList);
        }

        public int SetXTestStepValueAsTableCell(string value, int parentTestStepId, int tableId, bool searchByRow, string rowcolumnName, string cellName, string actionMode)
        {
            int testStepId1 = this.SetXTestStepValue("{NULL}", parentTestStepId, tableId, ActionMode.Select.ToString());
            Dictionary<string, int> childObjects = this.definitionObject.GetChildObjects(tableId);
            int num1 = searchByRow ? childObjects["<Row>"] : childObjects["<Col>"];
            int num2;
            if (string.IsNullOrEmpty(cellName))
            {
                num2 = this.SetXTestStepValueWithExplicitName(rowcolumnName, value, testStepId1, num1, actionMode);
            }
            else
            {
                int testStepId2 = this.SetXTestStepValueWithExplicitName(rowcolumnName, "{NULL}", testStepId1, num1, ActionMode.Select.ToString());
                num2 = this.SetXTestStepValueWithExplicitName(cellName, value, testStepId2, this.definitionObject.GetChildObjects(num1)["<Cell>"], actionMode);
            }
            return num2;
        }

        public int CreateManualTestStep(string name, int testCaseId, Dictionary<string, string> additionalProperties)
        {
            return this.definitionObject.AddObject(TCBusinessObject.ManualTestStep, new Dictionary<string, string>()
      {
        {
          TCBusinessObjectAttribute.Name,
          name
        }
      }, testCaseId);
        }

        public int CreateManualTestStepValue(string name, int manualTestStepId, string value, string actionMode, Dictionary<string, string> additionalProperties)
        {
            return this.definitionObject.AddObject(TCBusinessObject.ManualTestStepValue, new Dictionary<string, string>()
      {
        {
          TCBusinessObjectAttribute.ManualName,
          name
        },
        {
          TCBusinessObjectAttribute.Value,
          value
        },
        {
          TCBusinessObjectAttribute.ActionMode,
          actionMode
        }
      }, manualTestStepId);
        }

        public int CreateReusableTestStepBlock(string name, int testStepLibraryId)
        {
            return this.definitionObject.AddObject(TCBusinessObject.ReusableTestStepBlock, new Dictionary<string, string>()
      {
        {
          TCBusinessObjectAttribute.Name,
          name
        }
      }, testStepLibraryId);
        }

        public int CreateTestStepFolderReference(string name, int testCaseId, int reusableTestStepBlockId)
        {
            return this.definitionObject.AddObject(TCBusinessObject.TestStepFolderReference, new Dictionary<string, string>()
      {
        {
          TCBusinessObjectAttribute.Name,
          name
        }
      }, testCaseId, true, new Dictionary<string, string>()
      {
        {
          "ReusedItem",
          " -" + (object) reusableTestStepBlockId + " "
        }
      });
        }

        public int CreateTestSheet(string name, int parentId)
        {
            return this.definitionObject.AddObject(TCBusinessObject.TestSheet, new Dictionary<string, string>()
      {
        {
          TCBusinessObjectAttribute.Name,
          name
        }
      }, parentId);
        }

        public int CreateInstanceCollection(int parentId)
        {
            return this.definitionObject.AddObject(TCBusinessObject.Instances, new Dictionary<string, string>()
      {
        {
          TCBusinessObjectAttribute.Name,
          "Instances"
        }
      }, 0, true, new Dictionary<string, string>()
      {
        {
          "DefiningItem",
          " -" + (object) parentId + " "
        }
      });
        }

        public int CreateInstance(string name, int parentId)
        {
            return this.definitionObject.AddObject(TCBusinessObject.Instance, new Dictionary<string, string>()
      {
        {
          TCBusinessObjectAttribute.Name,
          name
        }
      }, parentId);
        }

        public int CreateTDAttribute(string name, int parentId)
        {
            return this.definitionObject.AddObject(TCBusinessObject.Attribute, new Dictionary<string, string>()
      {
        {
          TCBusinessObjectAttribute.Name,
          name
        },
        {
          "BusinessRelevant",
          "1"
        },
        {
          "TDM",
          "0"
        }
      }, parentId);
        }

        public void SetAttributeValue(string value, int attributeId, int instanceId)
        {
            this.definitionObject.AddObject(TCBusinessObject.InstanceValue, new Dictionary<string, string>()
      {
        {
          TCBusinessObjectAttribute.Value,
          value
        }
      }, 0, true, new Dictionary<string, string>()
      {
        {
          "Element",
          " -" + (object) attributeId + " "
        },
        {
          "Instance",
          " -" + (object) instanceId + " "
        }
      });
        }

        public int CreateBusinessParameter(string name, int parameterLayerId)
        {
            return this.definitionObject.AddObject("Parameter", new Dictionary<string, string>()
      {
        {
          TCBusinessObjectAttribute.Name,
          name
        }
      }, 0, true, new Dictionary<string, string>()
      {
        {
          "ParameterLayer",
          " -" + (object) parameterLayerId + " "
        }
      });
        }

        private int SetXTestStepValueWithExplicitName(string explicitName, string value, int testStepId, int moduleAttributeId, string actionMode)
        {
            Dictionary<string, string> attributeList = new Dictionary<string, string>();
            attributeList.Add("ExplicitName", explicitName);
            if (!value.Contains("="))
            {
                attributeList.Add(TCBusinessObjectAttribute.Value, value);
            }
            else
            {
                string[] strArray = value.Split(new char[1] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                attributeList.Add(TCBusinessObjectAttribute.ActionProperty, strArray[0]);
                attributeList.Add(TCBusinessObjectAttribute.Value, strArray[1]);
            }
            if (actionMode != null)
                attributeList.Add(TCBusinessObjectAttribute.ActionMode, actionMode);
            Dictionary<string, string> additionalPropertyList = new Dictionary<string, string>()
      {
        {
          "ModuleAttribute",
          " -" + (object) moduleAttributeId + " "
        }
      };
            return this.definitionObject.AddObject(TCBusinessObject.XTestStepValue, attributeList, testStepId, true, additionalPropertyList);
        }

        public void CreateIdParam(string paramName, string paramValue, int paramType, int parentId)
        {
            this.definitionObject.AddObject(TCBusinessObject.XParam, new Dictionary<string, string>()
      {
        {
          TCBusinessObjectAttribute.Name,
          paramName
        },
        {
          TCBusinessObjectAttribute.Value,
          paramValue
        },
        {
          TCBusinessObjectAttribute.ParamType,
          paramType.ToString()
        },
        {
          TCBusinessObjectAttribute.Visible,
          "true"
        }
      }, parentId);
        }

        public static class ParamType
        {
            public static int TechnicalIdParam = 5;
            public static int BusinessIdParam = 4;
            public static int ReflectedIdParam = 6;
            public static int ConfigurationParam = 8;
            public static int TransitionParam = 7;
            public static int SteeringParam = 2;
        }
    }
}
