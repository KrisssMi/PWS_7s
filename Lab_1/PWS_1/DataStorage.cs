using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PWS_1
{
    public class DataStorage
    {
        public Stack<int> Stack;
        public int ResultValue;

        public DataStorage()
        {
            Stack = new Stack<int>();
            ResultValue = 0;
        }

        public DataStorage(Stack<int> stack, int resultValue)
        {
            Stack = stack;
            ResultValue = resultValue;
        }

        public string Get()
        {
            int ret_result = ResultValue;
            if (Stack != null)
            {
                if (Stack.Count != 0)
                {
                    ret_result += Stack.Peek();
                }
            }
            string json = JsonConvert.SerializeObject(new { RESULT = ret_result });
            return json;
        }

        public void Post(int _result)
        {
            ResultValue = _result;
        }

        public void Put(int add)
        {
            Stack.Push(add);
        }

        public void Delete()
        {
            Stack.Pop();
        }

        public int StackElemsAmount()
        {
            return Stack.Count();
        }
    }
}