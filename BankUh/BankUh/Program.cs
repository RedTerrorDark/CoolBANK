using BankUh;

class Program
{
    static void Main(string[] args)
    {
        var bankLogin = new BankLogin();
        var db = AccountSystem.Instance;
        string? currentUsername = null;

        while (true)
        {
            Console.Clear();
            Console.WriteLine("====== ☺ BANK PIPIS-SPAM.NET ======");
            if (currentUsername == null)
            {
                Console.WriteLine("\n У вас есть аккаунт на PIPIS-SPAM.NET??\n Если у вас НЕТ аккаунта ЗАРЕГЕСРИРУЙТЕСЬ!!! если аккаунт есть... ну. войдите?\n\n 1. Зарегестрироватся \n 2. Я уже смешарик(войти)");
                var choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        bankLogin.RegisterUser(db);
                        break;
                    case "2":
                        currentUsername = bankLogin.LoginUser(db);
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("НЕТ!");
                        Thread.Sleep(500);
                        break;
                }
            }
            else
            {
                var me = db.Login(currentUsername, "");
                var myAccount = db.Accounts.First(a => a.Username == currentUsername);

                Console.WriteLine($"\n\nВы вошли как: {currentUsername}\nВаш ID счета: {myAccount.Id}\nБаланс: {myAccount.Balance} кромеров\n\n1. Перевести деньги\n2. Пополнить счет\n3. Вывести кромеры\n4. Выйти\n");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        bankLogin.MakeTransfer(db, currentUsername);
                        break;
                    case "2":
                        bankLogin.MakeAddKromer(db, currentUsername);
                        break;
                    case "3":
                        bankLogin.MakeNoKromer(db, currentUsername);
                        break;
                    case "4":
                        currentUsername = null;
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("НЕТ!");
                        Thread.Sleep(500);
                        break;
                }
            }
        }
    }
}