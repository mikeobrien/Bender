using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Tests.Performance
{
    [XmlType("Model")]
    [DataContract(Name = "Model", Namespace = "")]
    public class Model<T>
    {
        [DataMember]
        public string City { get; set; }

        [DataMember]
        public string Country { get; set; }

        [DataMember]
        public Father<T> Father { get; set; }

        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public List<Friend<T>> Friends { get; set; }

        [DataMember]
        public string LastName { get; set; }

        [DataMember]
        public string MiddleName { get; set; }

        [DataMember]
        public Mother<T> Mother { get; set; }

        [DataMember]
        public string Occupation { get; set; }

        [DataMember]
        public string Phone { get; set; }

        [DataMember]
        public string State { get; set; }

        [DataMember]
        public string Street { get; set; }

        [DataMember]
        public string Zip { get; set; }
    }

    [XmlType("Friend")]
    [DataContract(Name = "Friend", Namespace = "")]
    public class Friend<T>
    {
        [DataMember]
        public string City { get; set; }

        [DataMember]
        public string Country { get; set; }

        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string LastName { get; set; }

        [DataMember]
        public string MiddleName { get; set; }

        [DataMember]
        public string Occupation { get; set; }

        [DataMember]
        public string Phone { get; set; }

        [DataMember]
        public string State { get; set; }

        [DataMember]
        public string Street { get; set; }

        [DataMember]
        public string Zip { get; set; }
    }

    [XmlType("Mother")]
    [DataContract(Name = "Mother", Namespace = "")]
    public class Mother<T>
    {
        [DataMember]
        public string City { get; set; }

        [DataMember]
        public string Country { get; set; }

        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string LastName { get; set; }

        [DataMember]
        public string MiddleName { get; set; }

        [DataMember]
        public string Occupation { get; set; }

        [DataMember]
        public string Phone { get; set; }

        [DataMember]
        public string State { get; set; }

        [DataMember]
        public string Street { get; set; }

        [DataMember]
        public string Zip { get; set; }
    }

    [XmlType("Father")]
    [DataContract(Name = "Father", Namespace = "")]
    public class Father<T>
    {
        [DataMember]
        public string City { get; set; }

        [DataMember]
        public string Country { get; set; }

        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string LastName { get; set; }

        [DataMember]
        public string MiddleName { get; set; }

        [DataMember]
        public string Occupation { get; set; }

        [DataMember]
        public string Phone { get; set; }

        [DataMember]
        public string State { get; set; }

        [DataMember]
        public string Street { get; set; }

        [DataMember]
        public string Zip { get; set; }
    }
}