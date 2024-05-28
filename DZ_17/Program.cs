using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DZ_17
{
    internal class Program
    {

        public abstract class Car
        {
            public string Model { get; set; }
            protected int speed;
            public int position;
            public int finishLine = 100;

            public event EventHandler<CarFinishedEventArgs> CarFinished;

            public virtual void Drive()
            {
                position += speed;
                Console.WriteLine($"{Model} moved to position {position}");

                if (position >= finishLine)
                {
                    OnCarFinished(new CarFinishedEventArgs(this));
                }
            }

            protected virtual void OnCarFinished(CarFinishedEventArgs e)
            {
                CarFinished?.Invoke(this, e);
            }
        }
        public class SportsCar : Car
        {
            public SportsCar(string model)
            {
                Model = model;
                speed = new Random().Next(80, 150);
            }
        }

        public class PassengerCar : Car
        {
            public PassengerCar(string model)
            {
                Model = model;
                speed = new Random().Next(50, 100);
            }
        }

        public class Truck : Car
        {
            public Truck(string model)
            {
                Model = model;
                speed = new Random().Next(30, 80);
            }
        }

        public class Bus : Car
        {
            public Bus(string model)
            {
                Model = model;
                speed = new Random().Next(40, 90);
            }
        }
        public delegate void CarFinishedEventHandler(object sender, CarFinishedEventArgs e);

        public class CarFinishedEventArgs : EventArgs
        {
            public Car FinishedCar { get; }

            public CarFinishedEventArgs(Car car)
            {
                FinishedCar = car;
            }
        }

        public class RaceManager
        {
            public List<Car> cars = new List<Car>();

            public event CarFinishedEventHandler CarFinished;

            public void AddCar(Car car)
            {
                car.CarFinished += Car_CarFinished;
                cars.Add(car);
            }

            public void StartRace()
            {
                while (true)
                {
                    foreach (var car in cars)
                    {
                        car.Drive();
                    }
                }
            }

            private void Car_CarFinished(object sender, CarFinishedEventArgs e)
            {
                Console.WriteLine($"{e.FinishedCar.Model} has finished the race!");
                CarFinished?.Invoke(this, e);
            }
        }
        private static void RaceManager_CarFinished(object sender, CarFinishedEventArgs e)
        {
            if (((RaceManager)sender).cars.All(c => c.position >= c.finishLine))
            {
                Console.WriteLine("Race finished!");
                Environment.Exit(0);
            }
        }

        static void Main(string[] args)
        {
            var raceManager = new RaceManager();

            raceManager.AddCar(new SportsCar("Ferrari"));
            raceManager.AddCar(new PassengerCar("Toyota"));
            raceManager.AddCar(new Truck("Volvo"));
            raceManager.AddCar(new Bus("Mercedes"));

            raceManager.CarFinished += RaceManager_CarFinished;
            raceManager.StartRace();

        }
    }
}