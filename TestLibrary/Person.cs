using System;
using System.Collections.Generic;

namespace TestLibrary
{
    public enum Sex
    {
        Female = 0,
        Male = 1
    }

    public class Person
    {
        private string mName;
        private Sex mSex;
        private Int16 mAge;
        private bool mIsPerson;

        public Person()
        {
        }

        public Person(string name)
        {
            mName = name;
        }

        public Person(List<string> lll)
        {
            mName = lll != null && lll.Count > 0 ? lll[0] : "";
        }

        public Person(object[] lll)
        {
            mName = lll != null && lll.Length > 0 ? lll[0].ToString() : "";
        }

        public Person(Chinese[] lll)
        {
            mName = lll != null && lll.Length > 0 ? lll[0].Name : "";
        }

        public Person(Chinese cls)
        {
            mName = cls.Name;
        }

        public Person(List<Chinese> list)
        {
            if (list != null && list.Count > 0)
            {
                mName = list[0].Name;
            }
        }

        public Person(string name, Sex sex)
        {
            mName = name;
            mSex = sex;
        }

        public Person(string name, bool isPerson)
        {
            mName = name;
            mIsPerson = isPerson;
        }

        public Person(string name, Int16 age, bool isPerson)
        {
            mName = name;
            mAge = age;
            mIsPerson = isPerson;
        }

        public string Name
        {
            get
            {
                return mName;
            }
            set
            {
                mName = value;
            }
        }

        public Int16 Age
        {
            get
            {
                return mAge;
            }
            set
            {
                mAge = value;
            }
        }

        public bool IsPerson
        {
            get
            {
                return mIsPerson;
            }
            set
            {
                mIsPerson = value;
            }
        }

        public override string ToString()
        {
            return string.Concat("Name: ", mName, "  Sex: ", mSex, "  Age: ", mAge, "  IsPerson: ", mIsPerson);
        }
    }
}
