namespace TestExample
{
    public class Dog : IPetAnimal
    {
        string mName = string.Empty;
        string mDesc = string.Empty;

        public Dog()
        { 
        }

        public Dog(string name, string desc)
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
