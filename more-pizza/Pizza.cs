using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace more_pizza
{
    class Pizza
    {
        public int ID { get { return mID; } }
        public int Slices {  get { return mSlices; } }

        int mID;
        int mSlices;

        public Pizza(int id, int slices)
        {
            mID = id;
            mSlices = slices;
        }
    }
}
