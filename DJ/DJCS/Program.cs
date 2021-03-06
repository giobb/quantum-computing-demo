using System;

namespace DJCS
{
    class Program
    {
        static void Main()
        {            
            // Just for convenience, we use bool {false=0, true=1}
            // Using "b" for bit

            #region 1-Qubit oracles
            var oracles = new Func<bool, bool>[] {
                                            // Const
                                           _ => false,
                                           _ => true,
                                           // Balanced
                                           b0 => b0,
                                           b0 => !b0 };

            Console.WriteLine("1-bit oracles evaluation. (2^1)/2 + 1 = 2 queries needed");
            foreach (var oracle in oracles)
            {
                // NOTE: 2 queries needed
                var first = oracle(false);
                var second = oracle(true);

                WriteResult(first);
                WriteResult(second);
                Console.Write(" ");

                var resultBit = first == second ? "0 (Constant)" : "1 (Balanced)";

                Console.WriteLine($"Result bit is {resultBit}");
            }

            #endregion

            Console.WriteLine();
            Console.WriteLine();

            #region 2-Qbit oracles

            var twoBitOracles = new Func<bool,bool,bool>[] {
                                            // Const
                                            (_,__) => false, // I(b0) or do nothing
                                            (_,__) => true,  // X(b0)
                                            // Balanced
                                            (b0,b1) => b0,    // CNOT(b0,b2)
                                            (b0,b1) => !b0,   // CNOT(b0,b2), X(b2)
                                            (b0,b1) => b1,    // CNOT(b1,b2)               
                                            (b0,b1) => !b1,   // CONT(b1,b2), X(b2)
                                            (b0,b1) =>  {     
                                                            // CNOT(b0,b2)                                                       
                                                            bool b2 = b0;

                                                            // CNOT(b1,b2)
                                                            if (b1)
                                                                return !b2;
                                                            return b2;
                                                        },
                                            (b0,b1) =>  {    
                                                            // CNOT(b0,b2)                                                        
                                                            bool b2 = b0;

                                                            // CNOT(b1,b2), X(b2)
                                                            if (b1)
                                                                return b2;
                                                            return !b2;
                                                        }                                   
            };

            Console.WriteLine("2-bit oracles evaluation. (2^2)/2 + 1 = 3 queries needed");
            foreach (var oracle in twoBitOracles) 
            {  
                var first = oracle(false,false);
                var second = oracle(false,true);
                var third = oracle(true,true);        
                var fourth = oracle(true,false);     

                WriteResult(first);                
                WriteResult(second);
                WriteResult(third);
                WriteResult(fourth);   
                Console.Write(" ");

                var resultBit = first == second && second == third ? "0 (Constant)" : "1 (Balanced)";

                Console.WriteLine($"Result bit is {resultBit}");
            }

            #endregion
        }    
        
        static void WriteResult(bool res) => Console.Write(res ? "1" : "0");
    }
}
