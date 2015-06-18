using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClinTrialObject
{

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = true)]
    public partial class required_header_struct
    {
        private string download_dateField;
        private string link_textField;
        private string urlField;
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string download_date
        {
            get
            {
                return this.download_dateField;
            }
            set
            {
                this.download_dateField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string link_text
        {
            get
            {
                return this.link_textField;
            }
            set
            {
                this.link_textField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string url
        {
            get
            {
                return this.urlField;
            }
            set
            {
                this.urlField = value;
            }
        }
    }
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = true)]
    public partial class id_info_struct
    {
        private string org_study_idField;
        private string[] secondary_idField;
        private string nct_idField;
        private string[] nct_aliasField;
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string org_study_id
        {
            get
            {
                return this.org_study_idField;
            }
            set
            {
                this.org_study_idField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("secondary_id", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string[] secondary_id
        {
            get
            {
                return this.secondary_idField;
            }
            set
            {
                this.secondary_idField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string nct_id
        {
            get
            {
                return this.nct_idField;
            }
            set
            {
                this.nct_idField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("nct_alias", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string[] nct_alias
        {
            get
            {
                return this.nct_aliasField;
            }
            set
            {
                this.nct_aliasField = value;
            }
        }
    }
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = true)]
    public partial class sponsor_struct
    {
        private string agencyField;
        private string agency_classField;
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string agency
        {
            get
            {
                return this.agencyField;
            }
            set
            {
                this.agencyField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string agency_class
        {
            get
            {
                return this.agency_classField;
            }
            set
            {
                this.agency_classField = value;
            }
        }
    }
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = true)]
    public partial class sponsors_struct
    {
        private sponsor_struct lead_sponsorField;
        private sponsor_struct[] collaboratorField;
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public sponsor_struct lead_sponsor
        {
            get
            {
                return this.lead_sponsorField;
            }
            set
            {
                this.lead_sponsorField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("collaborator", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public sponsor_struct[] collaborator
        {
            get
            {
                return this.collaboratorField;
            }
            set
            {
                this.collaboratorField = value;
            }
        }
    }
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = true)]
    public partial class oversight_info_struct
    {
        private string[] authorityField;
        private string has_dmcField;
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("authority", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string[] authority
        {
            get
            {
                return this.authorityField;
            }
            set
            {
                this.authorityField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string has_dmc
        {
            get
            {
                return this.has_dmcField;
            }
            set
            {
                this.has_dmcField = value;
            }
        }
    }
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = true)]
    public partial class protocol_outcome_struct
    {
        private string measureField;
        private string time_frameField;
        private string safety_issueField;
        private string descriptionField;
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string measure
        {
            get
            {
                return this.measureField;
            }
            set
            {
                this.measureField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string time_frame
        {
            get
            {
                return this.time_frameField;
            }
            set
            {
                this.time_frameField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string safety_issue
        {
            get
            {
                return this.safety_issueField;
            }
            set
            {
                this.safety_issueField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }
    }
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = true)]
    public partial class enrollment_struct
    {
        private string typeField;
        private string[] textField;
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string[] Text
        {
            get
            {
                return this.textField;
            }
            set
            {
                this.textField = value;
            }
        }
    }
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = true)]
    public partial class arm_group_struct
    {
        private string arm_group_labelField;
        private string arm_group_typeField;
        private string descriptionField;
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string arm_group_label
        {
            get
            {
                return this.arm_group_labelField;
            }
            set
            {
                this.arm_group_labelField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string arm_group_type
        {
            get
            {
                return this.arm_group_typeField;
            }
            set
            {
                this.arm_group_typeField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }
    }
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = true)]
    public partial class intervention_struct
    {
        private string intervention_typeField;
        private string intervention_nameField;
        private string descriptionField;
        private string[] arm_group_labelField;
        private string[] other_nameField;
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string intervention_type
        {
            get
            {
                return this.intervention_typeField;
            }
            set
            {
                this.intervention_typeField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string intervention_name
        {
            get
            {
                return this.intervention_nameField;
            }
            set
            {
                this.intervention_nameField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("arm_group_label", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string[] arm_group_label
        {
            get
            {
                return this.arm_group_labelField;
            }
            set
            {
                this.arm_group_labelField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("other_name", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string[] other_name
        {
            get
            {
                return this.other_nameField;
            }
            set
            {
                this.other_nameField = value;
            }
        }
    }
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = true)]
    public partial class textblock_struct
    {
        private string textblockField;
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string textblock
        {
            get
            {
                return this.textblockField;
            }
            set
            {
                this.textblockField = value;
            }
        }
    }
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = true)]
    public partial class eligibility_struct
    {
        private textblock_struct study_popField;
        private string sampling_methodField;
        private textblock_struct criteriaField;
        private string genderField;
        private string minimum_ageField;
        private string maximum_ageField;
        private string healthy_volunteersField;
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public textblock_struct study_pop
        {
            get
            {
                return this.study_popField;
            }
            set
            {
                this.study_popField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string sampling_method
        {
            get
            {
                return this.sampling_methodField;
            }
            set
            {
                this.sampling_methodField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public textblock_struct criteria
        {
            get
            {
                return this.criteriaField;
            }
            set
            {
                this.criteriaField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string gender
        {
            get
            {
                return this.genderField;
            }
            set
            {
                this.genderField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string minimum_age
        {
            get
            {
                return this.minimum_ageField;
            }
            set
            {
                this.minimum_ageField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string maximum_age
        {
            get
            {
                return this.maximum_ageField;
            }
            set
            {
                this.maximum_ageField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string healthy_volunteers
        {
            get
            {
                return this.healthy_volunteersField;
            }
            set
            {
                this.healthy_volunteersField = value;
            }
        }
    }
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = true)]
    public partial class contact_struct
    {
        private string first_nameField;
        private string middle_nameField;
        private string last_nameField;
        private string degreesField;
        private string phoneField;
        private string phone_extField;
        private string emailField;
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string first_name
        {
            get
            {
                return this.first_nameField;
            }
            set
            {
                this.first_nameField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string middle_name
        {
            get
            {
                return this.middle_nameField;
            }
            set
            {
                this.middle_nameField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string last_name
        {
            get
            {
                return this.last_nameField;
            }
            set
            {
                this.last_nameField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string degrees
        {
            get
            {
                return this.degreesField;
            }
            set
            {
                this.degreesField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string phone
        {
            get
            {
                return this.phoneField;
            }
            set
            {
                this.phoneField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string phone_ext
        {
            get
            {
                return this.phone_extField;
            }
            set
            {
                this.phone_extField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string email
        {
            get
            {
                return this.emailField;
            }
            set
            {
                this.emailField = value;
            }
        }
    }
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = true)]
    public partial class investigator_struct
    {
        private string first_nameField;
        private string middle_nameField;
        private string last_nameField;
        private string degreesField;
        private string roleField;
        private string affiliationField;
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string first_name
        {
            get
            {
                return this.first_nameField;
            }
            set
            {
                this.first_nameField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string middle_name
        {
            get
            {
                return this.middle_nameField;
            }
            set
            {
                this.middle_nameField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string last_name
        {
            get
            {
                return this.last_nameField;
            }
            set
            {
                this.last_nameField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string degrees
        {
            get
            {
                return this.degreesField;
            }
            set
            {
                this.degreesField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string role
        {
            get
            {
                return this.roleField;
            }
            set
            {
                this.roleField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string affiliation
        {
            get
            {
                return this.affiliationField;
            }
            set
            {
                this.affiliationField = value;
            }
        }
    }
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = true)]
    public partial class address_struct
    {
        private string cityField;
        private string stateField;
        private string zipField;
        private string countryField;
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string city
        {
            get
            {
                return this.cityField;
            }
            set
            {
                this.cityField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string state
        {
            get
            {
                return this.stateField;
            }
            set
            {
                this.stateField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string zip
        {
            get
            {
                return this.zipField;
            }
            set
            {
                this.zipField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string country
        {
            get
            {
                return this.countryField;
            }
            set
            {
                this.countryField = value;
            }
        }
    }
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = true)]
    public partial class facility_struct
    {
        private string nameField;
        private address_struct addressField;
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public address_struct address
        {
            get
            {
                return this.addressField;
            }
            set
            {
                this.addressField = value;
            }
        }
    }
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = true)]
    public partial class location_struct
    {
        private facility_struct facilityField;
        private string statusField;
        private contact_struct contactField;
        private contact_struct contact_backupField;
        private investigator_struct[] investigatorField;
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public facility_struct facility
        {
            get
            {
                return this.facilityField;
            }
            set
            {
                this.facilityField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public contact_struct contact
        {
            get
            {
                return this.contactField;
            }
            set
            {
                this.contactField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public contact_struct contact_backup
        {
            get
            {
                return this.contact_backupField;
            }
            set
            {
                this.contact_backupField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("investigator", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public investigator_struct[] investigator
        {
            get
            {
                return this.investigatorField;
            }
            set
            {
                this.investigatorField = value;
            }
        }
    }
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = true)]
    public partial class countries_struct
    {
        private string[] countryField;
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("country", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string[] country
        {
            get
            {
                return this.countryField;
            }
            set
            {
                this.countryField = value;
            }
        }
    }
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = true)]
    public partial class link_struct
    {
        private string urlField;
        private string descriptionField;
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string url
        {
            get
            {
                return this.urlField;
            }
            set
            {
                this.urlField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }
    }
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = true)]
    public partial class reference_struct
    {
        private string citationField;
        private string pMIDField;
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string citation
        {
            get
            {
                return this.citationField;
            }
            set
            {
                this.citationField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string PMID
        {
            get
            {
                return this.pMIDField;
            }
            set
            {
                this.pMIDField = value;
            }
        }
    }
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = true)]
    public partial class responsible_party_struct
    {
        private string name_titleField;
        private string organizationField;
        private string responsible_party_typeField;
        private string investigator_affiliationField;
        private string investigator_full_nameField;
        private string investigator_titleField;
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string name_title
        {
            get
            {
                return this.name_titleField;
            }
            set
            {
                this.name_titleField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string organization
        {
            get
            {
                return this.organizationField;
            }
            set
            {
                this.organizationField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string responsible_party_type
        {
            get
            {
                return this.responsible_party_typeField;
            }
            set
            {
                this.responsible_party_typeField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string investigator_affiliation
        {
            get
            {
                return this.investigator_affiliationField;
            }
            set
            {
                this.investigator_affiliationField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string investigator_full_name
        {
            get
            {
                return this.investigator_full_nameField;
            }
            set
            {
                this.investigator_full_nameField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string investigator_title
        {
            get
            {
                return this.investigator_titleField;
            }
            set
            {
                this.investigator_titleField = value;
            }
        }
    }
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = true)]
    public partial class browse_struct
    {
        private string[] mesh_termField;
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("mesh_term", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string[] mesh_term
        {
            get
            {
                return this.mesh_termField;
            }
            set
            {
                this.mesh_termField = value;
            }
        }
    }
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = true)]
    public partial class group_struct
    {
        private string titleField;
        private string descriptionField;
        private string group_idField;
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string title
        {
            get
            {
                return this.titleField;
            }
            set
            {
                this.titleField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string group_id
        {
            get
            {
                return this.group_idField;
            }
            set
            {
                this.group_idField = value;
            }
        }
    }
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = true)]
    public partial class participants_struct
    {
        private string group_idField;
        private string countField;
        private string valueField;
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string group_id
        {
            get
            {
                return this.group_idField;
            }
            set
            {
                this.group_idField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string count
        {
            get
            {
                return this.countField;
            }
            set
            {
                this.countField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = true)]
    public partial class milestone_struct
    {
        private string titleField;
        private participants_struct[] participants_listField;
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string title
        {
            get
            {
                return this.titleField;
            }
            set
            {
                this.titleField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("participants", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public participants_struct[] participants_list
        {
            get
            {
                return this.participants_listField;
            }
            set
            {
                this.participants_listField = value;
            }
        }
    }
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = true)]
    public partial class period_struct
    {
        private string titleField;
        private milestone_struct[] milestone_listField;
        private milestone_struct[] drop_withdraw_reason_listField;
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string title
        {
            get
            {
                return this.titleField;
            }
            set
            {
                this.titleField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("milestone", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public milestone_struct[] milestone_list
        {
            get
            {
                return this.milestone_listField;
            }
            set
            {
                this.milestone_listField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("drop_withdraw_reason", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public milestone_struct[] drop_withdraw_reason_list
        {
            get
            {
                return this.drop_withdraw_reason_listField;
            }
            set
            {
                this.drop_withdraw_reason_listField = value;
            }
        }
    }
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = true)]
    public partial class participant_flow_struct
    {
        private string recruitment_detailsField;
        private string pre_assignment_detailsField;
        private group_struct[] group_listField;
        private period_struct[] period_listField;
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string recruitment_details
        {
            get
            {
                return this.recruitment_detailsField;
            }
            set
            {
                this.recruitment_detailsField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string pre_assignment_details
        {
            get
            {
                return this.pre_assignment_detailsField;
            }
            set
            {
                this.pre_assignment_detailsField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("group", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public group_struct[] group_list
        {
            get
            {
                return this.group_listField;
            }
            set
            {
                this.group_listField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("period", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public period_struct[] period_list
        {
            get
            {
                return this.period_listField;
            }
            set
            {
                this.period_listField = value;
            }
        }
    }
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = true)]
    public partial class measurement_struct
    {
        private string group_idField;
        private string valueField;
        private string spreadField;
        private string lower_limitField;
        private string upper_limitField;
        private string valueField1;
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string group_id
        {
            get
            {
                return this.group_idField;
            }
            set
            {
                this.group_idField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string spread
        {
            get
            {
                return this.spreadField;
            }
            set
            {
                this.spreadField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string lower_limit
        {
            get
            {
                return this.lower_limitField;
            }
            set
            {
                this.lower_limitField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string upper_limit
        {
            get
            {
                return this.upper_limitField;
            }
            set
            {
                this.upper_limitField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField1;
            }
            set
            {
                this.valueField1 = value;
            }
        }
    }
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = true)]
    public partial class measure_category_struct
    {
        private string sub_titleField;
        private measurement_struct[] measurement_listField;
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string sub_title
        {
            get
            {
                return this.sub_titleField;
            }
            set
            {
                this.sub_titleField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("measurement", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public measurement_struct[] measurement_list
        {
            get
            {
                return this.measurement_listField;
            }
            set
            {
                this.measurement_listField = value;
            }
        }
    }
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = true)]
    public partial class measure_struct
    {
        private string titleField;
        private string descriptionField;
        private string unitsField;
        private string paramField;
        private string dispersionField;
        private measure_category_struct[] category_listField;
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string title
        {
            get
            {
                return this.titleField;
            }
            set
            {
                this.titleField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string units
        {
            get
            {
                return this.unitsField;
            }
            set
            {
                this.unitsField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string param
        {
            get
            {
                return this.paramField;
            }
            set
            {
                this.paramField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string dispersion
        {
            get
            {
                return this.dispersionField;
            }
            set
            {
                this.dispersionField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("category", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public measure_category_struct[] category_list
        {
            get
            {
                return this.category_listField;
            }
            set
            {
                this.category_listField = value;
            }
        }
    }
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = true)]
    public partial class baseline_struct
    {
        private string populationField;
        private group_struct[] group_listField;
        private measure_struct[] measure_listField;
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string population
        {
            get
            {
                return this.populationField;
            }
            set
            {
                this.populationField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("group", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public group_struct[] group_list
        {
            get
            {
                return this.group_listField;
            }
            set
            {
                this.group_listField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("measure", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public measure_struct[] measure_list
        {
            get
            {
                return this.measure_listField;
            }
            set
            {
                this.measure_listField = value;
            }
        }
    }
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = true)]
    public partial class analysis_struct
    {
        private string[] group_id_listField;
        private string groups_descField;
        private string non_inferiorityField;
        private string non_inferiority_descField;
        private string p_valueField;
        private string p_value_descField;
        private string methodField;
        private string method_descField;
        private string param_typeField;
        private string param_valueField;
        private string dispersion_typeField;
        private string dispersion_valueField;
        private string ci_percentField;
        private string ci_n_sidesField;
        private string ci_lower_limitField;
        private string ci_upper_limitField;
        private string ci_upper_limit_na_commentField;
        private string estimate_descField;
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("group_id", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public string[] group_id_list
        {
            get
            {
                return this.group_id_listField;
            }
            set
            {
                this.group_id_listField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string groups_desc
        {
            get
            {
                return this.groups_descField;
            }
            set
            {
                this.groups_descField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string non_inferiority
        {
            get
            {
                return this.non_inferiorityField;
            }
            set
            {
                this.non_inferiorityField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string non_inferiority_desc
        {
            get
            {
                return this.non_inferiority_descField;
            }
            set
            {
                this.non_inferiority_descField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string p_value
        {
            get
            {
                return this.p_valueField;
            }
            set
            {
                this.p_valueField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string p_value_desc
        {
            get
            {
                return this.p_value_descField;
            }
            set
            {
                this.p_value_descField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string method
        {
            get
            {
                return this.methodField;
            }
            set
            {
                this.methodField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string method_desc
        {
            get
            {
                return this.method_descField;
            }
            set
            {
                this.method_descField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string param_type
        {
            get
            {
                return this.param_typeField;
            }
            set
            {
                this.param_typeField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string param_value
        {
            get
            {
                return this.param_valueField;
            }
            set
            {
                this.param_valueField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string dispersion_type
        {
            get
            {
                return this.dispersion_typeField;
            }
            set
            {
                this.dispersion_typeField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string dispersion_value
        {
            get
            {
                return this.dispersion_valueField;
            }
            set
            {
                this.dispersion_valueField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string ci_percent
        {
            get
            {
                return this.ci_percentField;
            }
            set
            {
                this.ci_percentField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string ci_n_sides
        {
            get
            {
                return this.ci_n_sidesField;
            }
            set
            {
                this.ci_n_sidesField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string ci_lower_limit
        {
            get
            {
                return this.ci_lower_limitField;
            }
            set
            {
                this.ci_lower_limitField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string ci_upper_limit
        {
            get
            {
                return this.ci_upper_limitField;
            }
            set
            {
                this.ci_upper_limitField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string ci_upper_limit_na_comment
        {
            get
            {
                return this.ci_upper_limit_na_commentField;
            }
            set
            {
                this.ci_upper_limit_na_commentField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string estimate_desc
        {
            get
            {
                return this.estimate_descField;
            }
            set
            {
                this.estimate_descField = value;
            }
        }
    }
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = true)]
    public partial class results_outcome_struct
    {
        private string typeField;
        private string titleField;
        private string descriptionField;
        private string time_frameField;
        private string safety_issueField;
        private string posting_dateField;
        private string populationField;
        private group_struct[] group_listField;
        private measure_struct[] measure_listField;
        private analysis_struct[] analysis_listField;
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string title
        {
            get
            {
                return this.titleField;
            }
            set
            {
                this.titleField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string time_frame
        {
            get
            {
                return this.time_frameField;
            }
            set
            {
                this.time_frameField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string safety_issue
        {
            get
            {
                return this.safety_issueField;
            }
            set
            {
                this.safety_issueField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string posting_date
        {
            get
            {
                return this.posting_dateField;
            }
            set
            {
                this.posting_dateField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string population
        {
            get
            {
                return this.populationField;
            }
            set
            {
                this.populationField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("group", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public group_struct[] group_list
        {
            get
            {
                return this.group_listField;
            }
            set
            {
                this.group_listField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("measure", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public measure_struct[] measure_list
        {
            get
            {
                return this.measure_listField;
            }
            set
            {
                this.measure_listField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("analysis", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public analysis_struct[] analysis_list
        {
            get
            {
                return this.analysis_listField;
            }
            set
            {
                this.analysis_listField = value;
            }
        }
    }
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = true)]
    public partial class vocab_term_struct
    {
        private string vocabField;
        private string valueField;
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string vocab
        {
            get
            {
                return this.vocabField;
            }
            set
            {
                this.vocabField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = true)]
    public partial class event_counts_struct
    {
        private string group_idField;
        private string subjects_affectedField;
        private string subjects_at_riskField;
        private string eventsField;
        private string valueField;
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string group_id
        {
            get
            {
                return this.group_idField;
            }
            set
            {
                this.group_idField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string subjects_affected
        {
            get
            {
                return this.subjects_affectedField;
            }
            set
            {
                this.subjects_affectedField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string subjects_at_risk
        {
            get
            {
                return this.subjects_at_riskField;
            }
            set
            {
                this.subjects_at_riskField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string events
        {
            get
            {
                return this.eventsField;
            }
            set
            {
                this.eventsField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = true)]
    public partial class event_struct
    {
        private vocab_term_struct sub_titleField;
        private string assessmentField;
        private string descriptionField;
        private event_counts_struct[] countsField;
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public vocab_term_struct sub_title
        {
            get
            {
                return this.sub_titleField;
            }
            set
            {
                this.sub_titleField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string assessment
        {
            get
            {
                return this.assessmentField;
            }
            set
            {
                this.assessmentField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("counts", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public event_counts_struct[] counts
        {
            get
            {
                return this.countsField;
            }
            set
            {
                this.countsField = value;
            }
        }
    }
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = true)]
    public partial class event_category_struct
    {
        private string titleField;
        private event_struct[] event_listField;
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string title
        {
            get
            {
                return this.titleField;
            }
            set
            {
                this.titleField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("event", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public event_struct[] event_list
        {
            get
            {
                return this.event_listField;
            }
            set
            {
                this.event_listField = value;
            }
        }
    }
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = true)]
    public partial class events_struct
    {
        private string frequency_thresholdField;
        private string default_vocabField;
        private string default_assessmentField;
        private event_category_struct[] category_listField;
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string frequency_threshold
        {
            get
            {
                return this.frequency_thresholdField;
            }
            set
            {
                this.frequency_thresholdField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string default_vocab
        {
            get
            {
                return this.default_vocabField;
            }
            set
            {
                this.default_vocabField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string default_assessment
        {
            get
            {
                return this.default_assessmentField;
            }
            set
            {
                this.default_assessmentField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("category", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public event_category_struct[] category_list
        {
            get
            {
                return this.category_listField;
            }
            set
            {
                this.category_listField = value;
            }
        }
    }
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = true)]
    public partial class reported_events_struct
    {
        private string time_frameField;
        private string descField;
        private group_struct[] group_listField;
        private events_struct serious_eventsField;
        private events_struct other_eventsField;
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string time_frame
        {
            get
            {
                return this.time_frameField;
            }
            set
            {
                this.time_frameField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string desc
        {
            get
            {
                return this.descField;
            }
            set
            {
                this.descField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("group", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public group_struct[] group_list
        {
            get
            {
                return this.group_listField;
            }
            set
            {
                this.group_listField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public events_struct serious_events
        {
            get
            {
                return this.serious_eventsField;
            }
            set
            {
                this.serious_eventsField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public events_struct other_events
        {
            get
            {
                return this.other_eventsField;
            }
            set
            {
                this.other_eventsField = value;
            }
        }
    }
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = true)]
    public partial class certain_agreements_struct
    {
        private string pi_employeeField;
        private string restrictive_agreementField;
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string pi_employee
        {
            get
            {
                return this.pi_employeeField;
            }
            set
            {
                this.pi_employeeField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string restrictive_agreement
        {
            get
            {
                return this.restrictive_agreementField;
            }
            set
            {
                this.restrictive_agreementField = value;
            }
        }
    }
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = true)]
    public partial class point_of_contact_struct
    {
        private string name_or_titleField;
        private string organizationField;
        private string phoneField;
        private string emailField;
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string name_or_title
        {
            get
            {
                return this.name_or_titleField;
            }
            set
            {
                this.name_or_titleField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string organization
        {
            get
            {
                return this.organizationField;
            }
            set
            {
                this.organizationField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string phone
        {
            get
            {
                return this.phoneField;
            }
            set
            {
                this.phoneField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string email
        {
            get
            {
                return this.emailField;
            }
            set
            {
                this.emailField = value;
            }
        }
    }
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = true)]
    public partial class clinical_results_struct
    {
        private participant_flow_struct participant_flowField;
        private baseline_struct baselineField;
        private results_outcome_struct[] outcome_listField;
        private reported_events_struct reported_eventsField;
        private certain_agreements_struct certain_agreementsField;
        private string limitations_and_caveatsField;
        private point_of_contact_struct point_of_contactField;
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public participant_flow_struct participant_flow
        {
            get
            {
                return this.participant_flowField;
            }
            set
            {
                this.participant_flowField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public baseline_struct baseline
        {
            get
            {
                return this.baselineField;
            }
            set
            {
                this.baselineField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("outcome", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public results_outcome_struct[] outcome_list
        {
            get
            {
                return this.outcome_listField;
            }
            set
            {
                this.outcome_listField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public reported_events_struct reported_events
        {
            get
            {
                return this.reported_eventsField;
            }
            set
            {
                this.reported_eventsField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public certain_agreements_struct certain_agreements
        {
            get
            {
                return this.certain_agreementsField;
            }
            set
            {
                this.certain_agreementsField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string limitations_and_caveats
        {
            get
            {
                return this.limitations_and_caveatsField;
            }
            set
            {
                this.limitations_and_caveatsField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public point_of_contact_struct point_of_contact
        {
            get
            {
                return this.point_of_contactField;
            }
            set
            {
                this.point_of_contactField = value;
            }
        }
    }
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = true)]
    public partial class date_struct
    {
        private string typeField;
        private string[] textField;
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string[] Text
        {
            get
            {
                return this.textField;
            }
            set
            {
                this.textField = value;
            }
        }
    }
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class clinical_study
    {
        private required_header_struct required_headerField;
        private id_info_struct id_infoField;
        private string brief_titleField;
        private string acronymField;
        private string official_titleField;
        private sponsors_struct sponsorsField;
        private string sourceField;
        private oversight_info_struct oversight_infoField;
        private textblock_struct brief_summaryField;
        private textblock_struct detailed_descriptionField;
        private string overall_statusField;
        private string why_stoppedField;
        private date_struct start_dateField;
        private date_struct end_dateField;
        private date_struct completion_dateField;
        private date_struct primary_completion_dateField;
        private string phaseField;
        private string study_typeField;
        private string study_designField;
        private string target_durationField;
        private protocol_outcome_struct[] primary_outcomeField;
        private protocol_outcome_struct[] secondary_outcomeField;
        private protocol_outcome_struct[] other_outcomeField;
        private string number_of_armsField;
        private string number_of_groupsField;
        private enrollment_struct enrollmentField;
        private string[] conditionField;
        private arm_group_struct[] arm_groupField;
        private intervention_struct[] interventionField;
        private string biospec_retentionField;
        private textblock_struct biospec_descrField;
        private eligibility_struct eligibilityField;
        private investigator_struct[] overall_officialField;
        private contact_struct overall_contactField;
        private contact_struct overall_contact_backupField;
        private location_struct[] locationField;
        private string[] location_countriesField;
        private string[] removed_countriesField;
        private link_struct[] linkField;
        private reference_struct[] referenceField;
        private reference_struct[] results_referenceField;
        private date_struct verification_dateField;
        private date_struct lastchanged_dateField;
        private date_struct firstreceived_dateField;
        private date_struct firstreceived_results_dateField;
        private responsible_party_struct responsible_partyField;
        private string[] keywordField;
        private string is_fda_regulatedField;
        private string is_section_801Field;
        private string has_expanded_accessField;
        private string[] condition_browseField;
        private string[] intervention_browseField;
        private clinical_results_struct clinical_resultsField;
        private string rankField;
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public required_header_struct required_header
        {
            get
            {
                return this.required_headerField;
            }
            set
            {
                this.required_headerField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public id_info_struct id_info
        {
            get
            {
                return this.id_infoField;
            }
            set
            {
                this.id_infoField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string brief_title
        {
            get
            {
                return this.brief_titleField;
            }
            set
            {
                this.brief_titleField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string acronym
        {
            get
            {
                return this.acronymField;
            }
            set
            {
                this.acronymField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string official_title
        {
            get
            {
                return this.official_titleField;
            }
            set
            {
                this.official_titleField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public sponsors_struct sponsors
        {
            get
            {
                return this.sponsorsField;
            }
            set
            {
                this.sponsorsField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string source
        {
            get
            {
                return this.sourceField;
            }
            set
            {
                this.sourceField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public oversight_info_struct oversight_info
        {
            get
            {
                return this.oversight_infoField;
            }
            set
            {
                this.oversight_infoField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public textblock_struct brief_summary
        {
            get
            {
                return this.brief_summaryField;
            }
            set
            {
                this.brief_summaryField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public textblock_struct detailed_description
        {
            get
            {
                return this.detailed_descriptionField;
            }
            set
            {
                this.detailed_descriptionField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string overall_status
        {
            get
            {
                return this.overall_statusField;
            }
            set
            {
                this.overall_statusField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string why_stopped
        {
            get
            {
                return this.why_stoppedField;
            }
            set
            {
                this.why_stoppedField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public date_struct start_date
        {
            get
            {
                return this.start_dateField;
            }
            set
            {
                this.start_dateField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public date_struct end_date
        {
            get
            {
                return this.end_dateField;
            }
            set
            {
                this.end_dateField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public date_struct completion_date
        {
            get
            {
                return this.completion_dateField;
            }
            set
            {
                this.completion_dateField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public date_struct primary_completion_date
        {
            get
            {
                return this.primary_completion_dateField;
            }
            set
            {
                this.primary_completion_dateField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string phase
        {
            get
            {
                return this.phaseField;
            }
            set
            {
                this.phaseField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string study_type
        {
            get
            {
                return this.study_typeField;
            }
            set
            {
                this.study_typeField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string study_design
        {
            get
            {
                return this.study_designField;
            }
            set
            {
                this.study_designField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string target_duration
        {
            get
            {
                return this.target_durationField;
            }
            set
            {
                this.target_durationField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("primary_outcome", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public protocol_outcome_struct[] primary_outcome
        {
            get
            {
                return this.primary_outcomeField;
            }
            set
            {
                this.primary_outcomeField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("secondary_outcome", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public protocol_outcome_struct[] secondary_outcome
        {
            get
            {
                return this.secondary_outcomeField;
            }
            set
            {
                this.secondary_outcomeField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("other_outcome", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public protocol_outcome_struct[] other_outcome
        {
            get
            {
                return this.other_outcomeField;
            }
            set
            {
                this.other_outcomeField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string number_of_arms
        {
            get
            {
                return this.number_of_armsField;
            }
            set
            {
                this.number_of_armsField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string number_of_groups
        {
            get
            {
                return this.number_of_groupsField;
            }
            set
            {
                this.number_of_groupsField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public enrollment_struct enrollment
        {
            get
            {
                return this.enrollmentField;
            }
            set
            {
                this.enrollmentField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("condition", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string[] condition
        {
            get
            {
                return this.conditionField;
            }
            set
            {
                this.conditionField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("arm_group", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public arm_group_struct[] arm_group
        {
            get
            {
                return this.arm_groupField;
            }
            set
            {
                this.arm_groupField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("intervention", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public intervention_struct[] intervention
        {
            get
            {
                return this.interventionField;
            }
            set
            {
                this.interventionField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string biospec_retention
        {
            get
            {
                return this.biospec_retentionField;
            }
            set
            {
                this.biospec_retentionField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public textblock_struct biospec_descr
        {
            get
            {
                return this.biospec_descrField;
            }
            set
            {
                this.biospec_descrField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public eligibility_struct eligibility
        {
            get
            {
                return this.eligibilityField;
            }
            set
            {
                this.eligibilityField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("overall_official", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public investigator_struct[] overall_official
        {
            get
            {
                return this.overall_officialField;
            }
            set
            {
                this.overall_officialField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public contact_struct overall_contact
        {
            get
            {
                return this.overall_contactField;
            }
            set
            {
                this.overall_contactField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public contact_struct overall_contact_backup
        {
            get
            {
                return this.overall_contact_backupField;
            }
            set
            {
                this.overall_contact_backupField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("location", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public location_struct[] location
        {
            get
            {
                return this.locationField;
            }
            set
            {
                this.locationField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("country", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public string[] location_countries
        {
            get
            {
                return this.location_countriesField;
            }
            set
            {
                this.location_countriesField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("country", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public string[] removed_countries
        {
            get
            {
                return this.removed_countriesField;
            }
            set
            {
                this.removed_countriesField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("link", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public link_struct[] link
        {
            get
            {
                return this.linkField;
            }
            set
            {
                this.linkField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("reference", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public reference_struct[] reference
        {
            get
            {
                return this.referenceField;
            }
            set
            {
                this.referenceField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("results_reference", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public reference_struct[] results_reference
        {
            get
            {
                return this.results_referenceField;
            }
            set
            {
                this.results_referenceField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public date_struct verification_date
        {
            get
            {
                return this.verification_dateField;
            }
            set
            {
                this.verification_dateField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public date_struct lastchanged_date
        {
            get
            {
                return this.lastchanged_dateField;
            }
            set
            {
                this.lastchanged_dateField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public date_struct firstreceived_date
        {
            get
            {
                return this.firstreceived_dateField;
            }
            set
            {
                this.firstreceived_dateField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public date_struct firstreceived_results_date
        {
            get
            {
                return this.firstreceived_results_dateField;
            }
            set
            {
                this.firstreceived_results_dateField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public responsible_party_struct responsible_party
        {
            get
            {
                return this.responsible_partyField;
            }
            set
            {
                this.responsible_partyField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("keyword", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string[] keyword
        {
            get
            {
                return this.keywordField;
            }
            set
            {
                this.keywordField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string is_fda_regulated
        {
            get
            {
                return this.is_fda_regulatedField;
            }
            set
            {
                this.is_fda_regulatedField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string is_section_801
        {
            get
            {
                return this.is_section_801Field;
            }
            set
            {
                this.is_section_801Field = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string has_expanded_access
        {
            get
            {
                return this.has_expanded_accessField;
            }
            set
            {
                this.has_expanded_accessField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("mesh_term", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public string[] condition_browse
        {
            get
            {
                return this.condition_browseField;
            }
            set
            {
                this.condition_browseField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("mesh_term", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public string[] intervention_browse
        {
            get
            {
                return this.intervention_browseField;
            }
            set
            {
                this.intervention_browseField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public clinical_results_struct clinical_results
        {
            get
            {
                return this.clinical_resultsField;
            }
            set
            {
                this.clinical_resultsField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string rank
        {
            get
            {
                return this.rankField;
            }
            set
            {
                this.rankField = value;
            }
        }
    }
}
