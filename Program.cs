using System;
using System.Collections.Generic;

namespace StructuralPatternsLab3;

// 1. ADAPTER
public interface ITarget { string GetRequest(); }
public class Adaptee { public string GetSpecificRequest() => "Specific request"; }
public class Adapter : ITarget { private Adaptee _adaptee; public Adapter(Adaptee adaptee) => _adaptee = adaptee; public string GetRequest() => $"Adapter: {_adaptee.GetSpecificRequest()}"; }

// 2. BRIDGE
public interface IDevice { void TurnOn(); void TurnOff(); void SetVolume(int volume); }
public class TV : IDevice { public void TurnOn() => Console.WriteLine("  TV: On"); public void TurnOff() => Console.WriteLine("  TV: Off"); public void SetVolume(int v) => Console.WriteLine($"  TV: Volume {v}"); }
public class Radio : IDevice { public void TurnOn() => Console.WriteLine("  Radio: On"); public void TurnOff() => Console.WriteLine("  Radio: Off"); public void SetVolume(int v) => Console.WriteLine($"  Radio: Volume {v}"); }
public abstract class RemoteControl { protected IDevice _device; public RemoteControl(IDevice device) => _device = device; public abstract void TurnOn(); public abstract void TurnOff(); public abstract void SetVolume(int volume); }
public class AdvancedRemote : RemoteControl { public AdvancedRemote(IDevice device) : base(device) { } public override void TurnOn() => _device.TurnOn(); public override void TurnOff() => _device.TurnOff(); public override void SetVolume(int volume) => _device.SetVolume(volume); }

// 3. COMPOSITE
public interface IGraphic { void Draw(); }
public class Circle : IGraphic { public void Draw() => Console.WriteLine("  Circle"); }
public class Square : IGraphic { public void Draw() => Console.WriteLine("  Square"); }
public class CompositeGraphic : IGraphic { private List<IGraphic> _children = new(); public void Add(IGraphic g) => _children.Add(g); public void Draw() { Console.WriteLine("  Group:"); foreach (var c in _children) c.Draw(); } }

// 4. DECORATOR
public interface ICoffee { string GetDescription(); double GetCost(); }
public class SimpleCoffee : ICoffee { public string GetDescription() => "Coffee"; public double GetCost() => 50; }
public abstract class CoffeeDecorator : ICoffee { protected ICoffee _coffee; public CoffeeDecorator(ICoffee coffee) => _coffee = coffee; public virtual string GetDescription() => _coffee.GetDescription(); public virtual double GetCost() => _coffee.GetCost(); }
public class MilkDecorator : CoffeeDecorator { public MilkDecorator(ICoffee coffee) : base(coffee) { } public override string GetDescription() => _coffee.GetDescription() + ", Milk"; public override double GetCost() => _coffee.GetCost() + 15; }
public class SugarDecorator : CoffeeDecorator { public SugarDecorator(ICoffee coffee) : base(coffee) { } public override string GetDescription() => _coffee.GetDescription() + ", Sugar"; public override double GetCost() => _coffee.GetCost() + 5; }

// 5. FACADE
public class CPU { public void Start() => Console.WriteLine("  CPU Start"); public void Shutdown() => Console.WriteLine("  CPU Shutdown"); }
public class Memory { public void Load() => Console.WriteLine("  Memory Load"); public void Free() => Console.WriteLine("  Memory Free"); }
public class HardDrive { public void Read() => Console.WriteLine("  HDD Read"); public void Write() => Console.WriteLine("  HDD Write"); }
public class ComputerFacade { private CPU _cpu = new(); private Memory _memory = new(); private HardDrive _hdd = new(); public void Start() { Console.WriteLine("  Starting PC..."); _cpu.Start(); _memory.Load(); _hdd.Read(); Console.WriteLine("  PC Ready!"); } public void Shutdown() { Console.WriteLine("  Shutting down..."); _cpu.Shutdown(); _memory.Free(); _hdd.Write(); Console.WriteLine("  PC Off!"); } }

// 6. FLYWEIGHT
public class TreeType { private string _name; private string _color; public TreeType(string name, string color) { _name = name; _color = color; } public void Draw(int x, int y) => Console.WriteLine($"  Tree {_name} ({_color}) at ({x},{y})"); }
public static class TreeFactory { private static Dictionary<string, TreeType> _types = new(); public static TreeType GetTreeType(string name, string color) { string key = $"{name}_{color}"; if (!_types.ContainsKey(key)) _types[key] = new TreeType(name, color); return _types[key]; } public static int Count => _types.Count; }
public class Forest { private List<(TreeType type, int x, int y)> _trees = new(); public void Plant(int x, int y, string name, string color) { var type = TreeFactory.GetTreeType(name, color); _trees.Add((type, x, y)); } public void Draw() { foreach (var t in _trees) t.type.Draw(t.x, t.y); } }

// 7. PROXY
public interface IImage { void Display(); }
public class RealImage : IImage { private string _file; public RealImage(string file) { _file = file; Console.WriteLine($"  Loading: {_file}"); } public void Display() => Console.WriteLine($"  Display: {_file}"); }
public class ProxyImage : IImage { private string _file; private RealImage _real; public ProxyImage(string file) => _file = file; public void Display() { if (_real == null) _real = new RealImage(_file); _real.Display(); } }

// MAIN
class Program
{
    static void Main()
    {
        Console.WriteLine("\n=== LAB 3: STRUCTURAL PATTERNS ===\n");

        Console.WriteLine("1. ADAPTER");
        ITarget adapter = new Adapter(new Adaptee());
        Console.WriteLine($"  {adapter.GetRequest()}\n");

        Console.WriteLine("2. BRIDGE");
        IDevice tv = new TV();
        RemoteControl remote = new AdvancedRemote(tv);
        remote.TurnOn();
        remote.SetVolume(5);
        remote.TurnOff();
        Console.WriteLine();

        Console.WriteLine("3. COMPOSITE");
        var group = new CompositeGraphic();
        group.Add(new Circle());
        group.Add(new Square());
        group.Add(new Square());
        group.Draw();
        Console.WriteLine();

        Console.WriteLine("4. DECORATOR");
        ICoffee coffee = new SimpleCoffee();
        coffee = new MilkDecorator(coffee);
        coffee = new SugarDecorator(coffee);
        Console.WriteLine($"  {coffee.GetDescription()}: {coffee.GetCost():C}\n");

        Console.WriteLine("5. FACADE");
        var computer = new ComputerFacade();
        computer.Start();
        computer.Shutdown();
        Console.WriteLine();

        Console.WriteLine("6. FLYWEIGHT");
        var forest = new Forest();
        forest.Plant(1, 1, "Oak", "Green");
        forest.Plant(2, 3, "Pine", "Green");
        forest.Plant(3, 5, "Oak", "Green");
        forest.Draw();
        Console.WriteLine($"  Unique types: {TreeFactory.Count}\n");

        Console.WriteLine("7. PROXY");
        IImage image = new ProxyImage("photo.jpg");
        image.Display();
        image.Display();

        Console.WriteLine("\n=== ALL 7 STRUCTURAL PATTERNS DEMONSTRATED ===\n");
    }
}