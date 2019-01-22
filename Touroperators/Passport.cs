using System;
using System.Runtime.Serialization;

namespace Touroperators
{
    [DataContract]
    public class Passport
    {

        [DataMember(Name = "standardversion")]
        public string Standardversion { get; set; }

        [DataMember(Name = "identifier")]
        public string Identifier { get; set; }

        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "creator")]
        public string Creator { get; set; }

        [DataMember(Name = "created")]
        public string Created { get; set; }

        [DataMember(Name = "modified")]
        public string Modified { get; set; }

        [DataMember(Name = "subject")]
        public string Subject { get; set; }

        [DataMember(Name = "format")]
        public string Format { get; set; }

        [DataMember(Name = "data")]
        public Datum[] Data { get; set; }

        [DataMember(Name = "structure")]
        public Structure[] Structure { get; set; }

        [DataMember(Name = "publisher")]
        public Publisher Publisher { get; set; }
    }

    [DataContract]
    public class Publisher
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "phone")]
        public string Phone { get; set; }

        [DataMember(Name = "mbox")]
        public string Mbox { get; set; }
    }


    [DataContract]
    public class Datum
    {
        [DataMember(Name = "source")]
        public Uri Source { get; set; }

        [DataMember(Name = "created")]
        public string Created { get; set; }

        [DataMember(Name = "provenance")]
        public string Provenance { get; set; }

        [DataMember(Name = "valid")]
        public string Valid { get; set; }

        [DataMember(Name = "structure")]
        public string Structure { get; set; }
    }

    [DataContract]
    public class Structure
    {
        [DataMember(Name = "source")]
        public Uri Source { get; set; }

        [DataMember(Name = "created")]
        public string Created { get; set; }
    }
}
