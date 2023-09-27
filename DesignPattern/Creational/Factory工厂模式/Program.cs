// See https://aka.ms/new-console-template for more information


using Factory;

// 简单工厂
Console.WriteLine("简单工厂");
var standardPhone = SimpleFactory.CreatePhone(PhoneEnum.Standard);
standardPhone.Call();
standardPhone.Play();

// 反射工厂
Console.WriteLine("反射工厂");
var phone = ReflectFactory.CreatePhone("Factory." + nameof(StandardPhone));
phone?.Call();
phone?.Play();

// 工厂方法
Console.WriteLine("工厂方法");
var randomFactory = new RandomSpecialFactory();
var randomPhone = randomFactory.CreatePhone();
randomPhone.Call();
randomPhone.Play();

var chinaFactory = new ProSpecialFactory();
var chinaPhone = chinaFactory.CreatePhone();
chinaPhone.Call();
chinaPhone.Play();


// 抽象工厂
Console.WriteLine("抽象工厂");
var bigFactory = new XiaomiFactory();
bigFactory.CreateStandardPhone();
bigFactory.CreateProPhone();
bigFactory.CreateMaxPhone();