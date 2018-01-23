using System;
using MC.Updater.Utils;
using Nito.AsyncEx;
using MC.Updater.Actions.Milestone;

namespace MC.Updater
{
    class Program
    {
        static Program()
        {
            new DI();
        }
        
        static void Main(string[] args)
        {

            AsyncContext.Run(() => ImageToS3.UsersDo());
        }
    }
}
