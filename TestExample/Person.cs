using System.Collections.Generic;

namespace TestExample
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
        private int mAge;
        private bool mIsChinese;

        public Person()
        {
        }

        public Person(string name)
        {
            mName = name;
        }

        public Person(string name, string[] usedNames)
            : this(name)
        {
            this.UsedNames = usedNames;
        }

        public Person(string name, Sex sex)
        {
            mName = name;
            mSex = sex;
        }

        public Person(string name, bool isChinese)
        {
            mName = name;
            mIsChinese = isChinese;
        }

        public Person(string name, int age, bool isChinese)
        {
            mName = name;
            mAge = age;
            mIsChinese = isChinese;
        }

        public Person(Person father, Person mother)
        {
            this.Father = father;
            this.Mother = mother;
        }

        public Person(string name, List<string> hobbies)
            : this(name)
        {
            this.Hobbies = hobbies;
        }

        public Person(string name, List<Person> children)
            : this(name)
        {
            this.Children = children;
        }

        public Person(string name, Person[] children)
            : this(name)
        {
            this.Children = new List<Person>(children);
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

        public List<string> Hobbies { get; set; } = null;

        public string[] UsedNames { get; set; } = null;

        public Sex Sex
        {
            get
            {
                return mSex;
            }
            set
            {
                mSex = value;
            }
        }

        public int Age
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

        public virtual bool IsChinese
        {
            get
            {
                return mIsChinese;
            }
            set
            {
                mIsChinese = value;
            }
        }

        public Person Father { get; set; } = null;

        public Person Mother { get; set; } = null;

        public List<Person> Children { get; set; } = null;

        public IPetAnimal[] PetAnimals { get; set; } = null;

        public override string ToString()
        {
            return string.Concat("Name: ", this.Name, "  Sex: ", this.Sex, "  Age: ", this.Age, "  IsChinese: ", this.IsChinese);
        }
    }
}
