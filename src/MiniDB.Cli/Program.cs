using MiniDB.Application.Services;
using MiniDB.Application.UseCases;
using MiniDB.Domain;
using MiniDB.Infrastructure.Storage;

var dbPath = Path.Combine(Directory.GetCurrentDirectory(), "Data");

var storage = new FileStorageEngine(dbPath);
var dbService = new DatabaseService(storage, dbPath);

var createUseCase = new CreateUseCase(dbService);
var insertUseCase = new InsertUseCase(dbService);
var selectUseCase = new SelectAllUseCase(dbService);
var updateUseCase = new UpdateUseCase(dbService);
var deleteUseCase = new DeleteUseCase(dbService);

while (true)
{
  Console.WriteLine();
  Console.WriteLine("1 - Create Table");
  Console.WriteLine("2 - Insert Row");
  Console.WriteLine("3 - Select All");
  Console.WriteLine("4 - Update Row");
  Console.WriteLine("5 - Delete Row");
  Console.WriteLine("0 - Exit");

  Console.Write("Choose: ");
  var input = Console.ReadLine();

  switch (input)
  {
    case "1":
      CreateTable();
      break;

    case "2":
      InsertRow();
      break;

    case "3":
      SelectAll();
      break;

    case "4":
      UpdateRow();
      break;

    case "5":
      DeleteRow();
      break;

    case "0":
      return;
  }

  void CreateTable()
  {
    var columns = new List<Column>
    {
        new Column("Id", DataType.Int),
        new Column("Name", DataType.String, 50)
    };

    createUseCase.Execute("Users", columns);
    Console.WriteLine("Table 'Users' created.");
  }

  void InsertRow()
  {
    Console.Write("Id: ");
    var id = int.Parse(Console.ReadLine()!);

    Console.Write("Name: ");
    var name = Console.ReadLine()!;

    insertUseCase.Execute("Users", new object[] { id, name });

    Console.WriteLine("Row inserted.");
  }

  void SelectAll()
  {
    var rows = selectUseCase.Execute("Users");

    Console.WriteLine("Rows:");

    foreach (var row in rows)
    {
      Console.WriteLine($"{row.GetValue(0)} | {row.GetValue(1)}");
    }
  }

  void UpdateRow()
  {
    Console.Write("Id to update: ");
    var id = int.Parse(Console.ReadLine()!);

    Console.Write("New Name: ");
    var name = Console.ReadLine()!;

    updateUseCase.Execute(
        "Users",
        r => (int)r.GetValue(0)! == id,
        r => new object[] { id, name }
    );

    Console.WriteLine("Row updated.");
  }

  void DeleteRow()
  {
    Console.Write("Id to delete: ");
    var id = int.Parse(Console.ReadLine()!);

    deleteUseCase.Execute(
        "Users",
        r => (int)r.GetValue(0)! == id
    );

    Console.WriteLine("Row deleted.");
  }
}