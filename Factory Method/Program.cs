using System;
using System.Collections.Generic;

namespace FactoryMethodHiringSystem
{
    // ============================================================
    // 1. ПРОДУКТ (ИНТЕРФЕЙС СОТРУДНИКА)
    // ============================================================
    // Это общий тип для ВСЕХ сотрудников.
    // Клиент будет работать только через этот интерфейс.
    public interface IEmployee
    {
        string Name { get; }
        int Experience { get; }
        double BaseSalary { get; }

        void Work();
        double CalculateSalary();
        void ShowInfo();
    }

    // ============================================================
    // 2. БАЗОВЫЙ КЛАСС СОТРУДНИКА (общая логика)
    // ============================================================

    public abstract class EmployeeBase : IEmployee
    {
        public string Name { get; protected set; }
        public int Experience { get; protected set; }
        public double BaseSalary { get; protected set; }

        public EmployeeBase(string name, int experience, double baseSalary)
        {
            Name = name;
            Experience = experience;
            BaseSalary = baseSalary;
        }

        // Каждый сотрудник работает по-разному
        public abstract void Work();

        // Общая формула расчёта зарплаты
        public virtual double CalculateSalary()
        {
            // За каждый год опыта +5%
            double bonus = BaseSalary * 0.05 * Experience;
            return BaseSalary + bonus;
        }

        public virtual void ShowInfo()
        {
            Console.WriteLine("-------------");
            Console.WriteLine($"Name: {Name}");
            Console.WriteLine($"Experience: {Experience} years");
            Console.WriteLine($"Final Salary: {CalculateSalary()}$");
        }
    }

    // ============================================================
    // 3. КОНКРЕТНЫЕ ПРОДУКТЫ
    // ============================================================

    public class Developer : EmployeeBase
    {
        public Developer(string name, int experience)
            : base(name, experience, 4000)
        {
        }

        public override void Work()
        {
            Console.WriteLine("Writing code and fixing bugs.");
        }

        public override double CalculateSalary()
        {
            // Разработчики получают доп. бонус 1000$
            return base.CalculateSalary() + 1000;
        }
    }

    public class Manager : EmployeeBase
    {
        public Manager(string name, int experience)
            : base(name, experience, 5000)
        {
        }

        public override void Work()
        {
            Console.WriteLine("Managing team and planning projects.");
        }

        public override double CalculateSalary()
        {
            // Менеджеры получают 10% премии
            return base.CalculateSalary() * 1.10;
        }
    }

    public class Designer : EmployeeBase
    {
        public Designer(string name, int experience)
            : base(name, experience, 3500)
        {
        }

        public override void Work()
        {
            Console.WriteLine("Designing interfaces and prototypes.");
        }
    }

    // ============================================================
    // 4. CREATOR (ОТДЕЛ НАЙМА)
    // ============================================================
    // ВАЖНО: здесь находится Factory Method

    public abstract class HiringDepartment
    {
        protected List<IEmployee> employees = new List<IEmployee>();

        // Factory Method
        // Подклассы решают, какого сотрудника создать
        protected abstract IEmployee CreateEmployee(string name, int experience);

        // Общий алгоритм найма
        public void Hire(string name, int experience)
        {
            Console.WriteLine("\nStarting hiring process...");

            IEmployee employee = CreateEmployee(name, experience);

            Console.WriteLine("Employee created.");
            employee.ShowInfo();
            employee.Work();

            employees.Add(employee);

            Console.WriteLine("Employee successfully hired!\n");
        }

        public void ShowAllEmployees()
        {
            Console.WriteLine("\n=== Company Employees ===");

            foreach (var emp in employees)
            {
                emp.ShowInfo();
            }
        }
    }

    // ============================================================
    // 5. КОНКРЕТНЫЕ ОТДЕЛЫ (ПЕРЕОПРЕДЕЛЯЮТ FACTORY METHOD)
    // ============================================================

    public class DeveloperDepartment : HiringDepartment
    {
        protected override IEmployee CreateEmployee(string name, int experience)
        {
            return new Developer(name, experience);
        }
    }

    public class ManagementDepartment : HiringDepartment
    {
        protected override IEmployee CreateEmployee(string name, int experience)
        {
            return new Manager(name, experience);
        }
    }

    public class DesignDepartment : HiringDepartment
    {
        protected override IEmployee CreateEmployee(string name, int experience)
        {
            return new Designer(name, experience);
        }
    }

    // ============================================================
    // 6. MAIN
    // ============================================================

    public class Program
    {
        public static void Main()
        {
            while (true)
            {
                Console.WriteLine("Enter employee name (or 'exit' for quit):");
                string name = Console.ReadLine();
                if (name.ToLower() == "exit") break;

                Console.WriteLine("Enter employee experience (in years):");
                int experience = int.Parse(Console.ReadLine());

                Console.WriteLine("Department:");
                Console.WriteLine("1 - Developers");
                Console.WriteLine("2 - Managers");
                Console.WriteLine("3 - Designers");

                string choice = Console.ReadLine();
                HiringDepartment department;

                if (choice == "1")
                    department = new DeveloperDepartment();
                else if (choice == "2")
                    department = new ManagementDepartment();
                else
                    department = new DesignDepartment();

                department.Hire(name, experience);
            }

            Console.WriteLine("Program finished.");
        }
    }
}