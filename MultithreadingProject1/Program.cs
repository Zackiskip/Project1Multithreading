using System;

namespace MultithreadingProject1
{
    class Program
    {
        public static double balance =0;
        public static int mutex=0;
        static void Main(string[] args)
        {
            int running=1;
            while(running==1){
                Thread moneyThread;

                Console.WriteLine("Pick an option:");
                Console.WriteLine("1.) Deposit:");
                Console.WriteLine("2.) Withdraw:");
                Console.WriteLine("3.) View current balance:");
                Console.WriteLine("4.) Exit:");
                Console.WriteLine("Money is processed in increments of $100 after selecting an option it takes time to process");
                int choice = Convert.ToInt32(Console.ReadLine());
                switch(choice){
                    case 1:
                        Console.WriteLine("Enter the amount you would like to deposit:");
                        double deposit = Convert.ToDouble(Console.ReadLine());
                        moneyThread = new Thread(() => Deposit(deposit));
                        moneyThread.Start();
                        break;
                    case 2:
                        Console.WriteLine("Enter the amount you would like to withdraw:");
                        double withdraw = Convert.ToDouble(Console.ReadLine());
                        moneyThread = new Thread(() => Withdraw(withdraw));
                        moneyThread.Start();
                        break;
                    case 3:
                        Console.WriteLine("Your current balance is: $" + balance);
                        break;
                    case 4:
                        running = 0;
                        Console.WriteLine("Goodbye");
                        break;
                    default:
                        Console.WriteLine("Invalid option");
                        break;
                }

            }            
            
        }
        static void Deposit(double depositAmount)
        {
            mutex++;
            
            while(depositAmount>=100)
            {
                depositAmount-=100;
                balance+=100;
                Thread.Sleep(100);
                
            }
            balance=(depositAmount%100)+balance;//adds remainder to balance that is under 100
            mutex--;
            Console.WriteLine("Deposit complete");
        }
        static void Withdraw(double withdrawAmount)
        {
            int timeoutDetection=0;
            while(mutex>0){
                {
                    if (timeoutDetection>120){//if a thread is held for 2 minutes then it will be cancelled to prevent deadlock
                        Console.WriteLine("Timeout detected");
                        Console.WriteLine("Thread stalled too long, cancelling withdraw of $"+withdrawAmount);
                        
                        return;
                    }
                    timeoutDetection++;
                   Thread.Sleep(1000);//holds a withdraw thread until all deposists AND withdrawls are complete to not overdraw account
                }
                
            }
                if(withdrawAmount>balance)
                {
                    //mutex++; //uncomment the mutex++ to introduce deadlock if someone tries to withdraw more than the balance
                    Console.WriteLine("Insufficient funds");
                }
                else
                {
                    mutex++;
                    for(int i=0;i<10;i++)
                    {
                        balance-=withdrawAmount;
                        Thread.Sleep(100);
                    }
                    Console.WriteLine("Withdraw complete");
                    mutex--;
                }
            
        }
    }
}