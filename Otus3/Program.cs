using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using Microsoft.Extensions.Caching.Memory;
using Otus3.Abstractions;

namespace Otus3
{
    public class Program
    {
        public static void Main()
        {
            var app = new BudgetApplication(
                new InMemoryTransactionRepository(),
                new TransactionParser(),
                new ExchangeRatesApiConverter(
                    new HttpClient(),
                    new MemoryCache(
                        new MemoryCacheOptions()),
                    "13edf40bad500a9eb2d99dc70ca1a518"));

            //var transactions = new List<string>
            //{
            //    "Доход 15000 RUB Зарплата OTUS 2021-05-01",
            //    "Трата -400 RUB Продукты Пятерочка 2021-05-02",
            //    "Перевод -1500 RUB Серёга Долг 2021-05-02",
            //    "Трата -2000 RUB Бензин IRBIS 2021-05-03",
            //    "Трата -500 RUB Кафе Шоколадница 2021-05-04",
            //    "Трата -59 EUR Развлечения Steam 2021-05-05"
            //};

            while (true)
            {
                Console.WriteLine("Введите команду:");
                Console.WriteLine("1: Добавить транзакцию");
                Console.WriteLine("2: Загрузить транзакции из файла");
                Console.WriteLine("3: Вывести список транзакций");
                Console.WriteLine("4: Вывести баланс в валюте");
                Console.WriteLine("q: Выход");
                Console.Write("Команда: ");

                var cmd = Console.ReadLine();

                var quit = false;

                switch (cmd)
                {
                    case "1":
                        AddTransaction(app);
                        break;
                    case "2":
                        LoadTransaction(app);
                        break;
                    case "3":
                        OutputTransactions(app);
                        break;
                    case "4":
                        OutputBalanceInCurrency(app);
                        break;
                    case "q":
                        quit = true;
                        break;
                }

                Console.WriteLine();

                if (quit)
                {
                    break;
                }
            }


            //foreach (var transaction in transactions)
            //{
            //    app.AddTransation(transaction);
            //}

            //app.OutputBalanceInCurrency("RUB");
        }

        private static void AddTransaction(IBudgetApplication app)
        {
            Console.WriteLine("1: Введите транзакцию в формате \"Операция Сумма Категория Назначение Дата(YYYY-MM-DD)\":");
            var input = Console.ReadLine();
            app.AddTransation(input);
        }

        private static void LoadTransaction(IBudgetApplication app)
        {
            Console.WriteLine("1: Введите полный путь к файлу (по-умолчанию transactions.txt):");
            var path = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(path))
            {
                path = "transactions.txt";
            }

            var streamReader = File.OpenText(path);

            while (!streamReader.EndOfStream)
            {
                var input = streamReader.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                {
                    continue;
                }

                app.AddTransation(input);
            }
        }

        private static void OutputTransactions(IBudgetApplication app)
        {
            app.OutputTransactions();
        }

        private static void OutputBalanceInCurrency(IBudgetApplication app)
        {
            Console.WriteLine("1: Введите трехсимвольный код валюты:");
            var currencyCode = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(currencyCode))
            {
                currencyCode = currencyCode.ToUpper();
            }

            app.OutputBalanceInCurrency(currencyCode);
        }
    }

    public class TransactionsFileReader: IEnumerable<string>
    {
        private readonly string _path;

        public TransactionsFileReader(string path)
        {
            _path = path;
        }

        public IEnumerator<string> GetEnumerator()
        {
            return new FileReaderEnumerator(_path);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class FileReaderEnumerator: IEnumerator<string>
    {
        private readonly string _path;

        private StreamReader _streamReader;

        public FileReaderEnumerator(string path)
        {
            _path = path;
        }

        public bool MoveNext()
        {
            _streamReader ??= File.OpenText(_path);

            if (!_streamReader.EndOfStream)
            {
                Current = _streamReader.ReadLine();
                return true;
            }

            return false;
        }

        public void Reset()
        {
            _streamReader?.Close();
            _streamReader = null;
            Current = null;
        }

        public string Current { get; private set; }

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            _streamReader?.Close();
        }
    }
}
