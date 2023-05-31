System.Timers.Timer timer = new System.Timers.Timer(5000);
timer.Elapsed += (s, e) =>
{
    Console.WriteLine("callback");
};

Console.WriteLine("start");
timer.Start();  
Console.ReadLine();