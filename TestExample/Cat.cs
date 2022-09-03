namespace TestExample
{
    public class Cat : IPetAnimal
    {
        string mName = string.Empty;
        string mDesc = string.Empty;

        public Cat()
        {
        }

        public Cat(string name, string desc)
        {
            this.mName = name;
            this.mDesc = desc;
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

        public string Description
        {
            get
            {
                return mDesc;
            }
            set
            {
                mDesc = value;
            }
        }
    }
}
