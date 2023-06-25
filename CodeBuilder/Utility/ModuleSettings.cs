namespace Flextronics.Applications.Library.Utility
{
    using System;
    using System.Xml.Serialization;

    public class ModuleSettings
    {
        private string _appframe = "f3";
        private string _blltype = "sql";
        private string _daltype = "sql";
        private string _dbHelperName = "DbHelperSQL";
        private string _editfont = "新宋体";
        private float _editfontsize = 9f;
        private string _folder = "Folder";
        private string _namespace = "Maticsoft";
        private string _procprefix;
        private string _projectname;

        [XmlElement]
        public string AppFrame
        {
            get
            {
                return this._appframe;
            }
            set
            {
                this._appframe = value;
            }
        }

        [XmlElement]
        public string BLLType
        {
            get
            {
                return this._blltype;
            }
            set
            {
                this._blltype = value;
            }
        }

        [XmlElement]
        public string DALType
        {
            get
            {
                return this._daltype;
            }
            set
            {
                this._daltype = value;
            }
        }

        [XmlElement]
        public string DbHelperName
        {
            get
            {
                return this._dbHelperName;
            }
            set
            {
                this._dbHelperName = value;
            }
        }

        [XmlElement]
        public string EditFont
        {
            get
            {
                return this._editfont;
            }
            set
            {
                this._editfont = value;
            }
        }

        [XmlElement]
        public float EditFontSize
        {
            get
            {
                return this._editfontsize;
            }
            set
            {
                this._editfontsize = value;
            }
        }

        [XmlElement]
        public string Folder
        {
            get
            {
                return this._folder;
            }
            set
            {
                this._folder = value;
            }
        }

        [XmlElement]
        public string Namepace
        {
            get
            {
                return this._namespace;
            }
            set
            {
                this._namespace = value;
            }
        }

        [XmlElement]
        public string ProcPrefix
        {
            get
            {
                return this._procprefix;
            }
            set
            {
                this._procprefix = value;
            }
        }

        [XmlElement]
        public string ProjectName
        {
            get
            {
                return this._projectname;
            }
            set
            {
                this._projectname = value;
            }
        }
    }
}

