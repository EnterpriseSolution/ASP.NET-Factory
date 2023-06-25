namespace Codematic
{
    using System;
    using System.Xml.Serialization;

    public class DbSettings
    {
        private bool _connectSimple;
        private string _connectstr;
        private string _dbname;
        private string _dbtype;
        private string _server;

        [XmlElement]
        public bool ConnectSimple
        {
            get
            {
                return this._connectSimple;
            }
            set
            {
                this._connectSimple = value;
            }
        }

        [XmlElement]
        public string ConnectStr
        {
            get
            {
                return this._connectstr;
            }
            set
            {
                this._connectstr = value;
            }
        }

        [XmlElement]
        public string DbName
        {
            get
            {
                return this._dbname;
            }
            set
            {
                this._dbname = value;
            }
        }

        [XmlElement]
        public string DbType
        {
            get
            {
                return this._dbtype;
            }
            set
            {
                this._dbtype = value;
            }
        }

        [XmlElement]
        public string Server
        {
            get
            {
                return this._server;
            }
            set
            {
                this._server = value;
            }
        }
    }
}

