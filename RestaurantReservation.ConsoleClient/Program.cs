
using Newtonsoft.Json;
using RestaurantReservation.ConsoleClient;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;

HttpClient client = new();
client.BaseAddress = new Uri("https://localhost:7141");
client.DefaultRequestHeaders.Accept.Clear();
client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

HttpResponseMessage response = await client.GetAsync("api/reservations/all");
response.EnsureSuccessStatusCode();

if(response.IsSuccessStatusCode)
{
    int choice = 0;
    bool keepRunning = true;

    while(keepRunning)
    {
        Console.WriteLine();
        Console.WriteLine("-----------Reservations interface-----------");
        Console.WriteLine("1. View all reservations");
        Console.WriteLine("2. Get reservation details by Id");
        Console.WriteLine("3. Create a reservation");
        Console.WriteLine("4. Update a reservation");
        Console.WriteLine("5. Delete a reservation");
        Console.WriteLine("100. Close reservation interface");
        choice = int.Parse(Console.ReadLine());

        switch(choice)
        {
            case 1:
                await AllReservations();
                break;

            case 2:
                await ReservationDetails();
                break;

            case 3:
                await CreateReservation();
                break;

            case 4:
                await UpdateReservation();
                break;

            case 5:
                await DeleteReservation();
                break;

            case 100:
                keepRunning = false;
                Console.WriteLine("!!! Reservations console is closed !!!");
                break;

            default:
                break;

        }
    }
}
else
{
    Console.WriteLine(response.StatusCode.ToString());
}

Console.ReadKey();

async Task AllReservations()
{
    response = await client.GetAsync("api/reservations/all");

    var reservations = await response.Content.ReadFromJsonAsync<IEnumerable<Reservation>>();

    foreach(Reservation res in reservations)
    {
        Console.WriteLine(res.Id+". "+res.Name);
    }
}

async Task ReservationDetails()
{
    await AllReservations();

    Console.Write("Choose a reservation by id to view it's details: ");
    int id =  int.Parse(Console.ReadLine());

    response = await client.GetAsync($"api/reservations/details/{id}");
    response.EnsureSuccessStatusCode();

    var res = await response.Content.ReadFromJsonAsync<Reservation>();
    Console.WriteLine("ID: " + res.Id +
                "\nName: " + res.Name +
                "\nTableFor: " + res.TableFor +
                "\nTableNo: " + res.TableNo +
                "\nTime: " + res.Time +
                "\nCreated: " + res.Created);

}

async Task CreateReservation()
{
    Reservation res = new();

    Console.Write("Name: ");
    res.Name = Console.ReadLine();

    Console.Write("Table for how many: ");
    res.TableFor = int.Parse(Console.ReadLine());

    Console.Write("Table Number: ");
    res.TableNo = int.Parse(Console.ReadLine());

    Console.Write("Check-in time(yyyy-mm-dd hh:mm PM/AM): ");
    res.Time = DateTime.Parse(Console.ReadLine());

    res.Created = DateTime.Now;

    var payload = JsonConvert.SerializeObject(res);
    var content = new StringContent(payload, Encoding.UTF8,"application/json");

    response = await client.PostAsync("api/reservations/create", content);
    response.EnsureSuccessStatusCode();

    Console.WriteLine("!!! Reservation created successfully !!!");

}

async Task DeleteReservation()
{
    await AllReservations();

    Console.Write("Choose a reservation by id to delete: ");
    int id = int.Parse(Console.ReadLine());

    response = await client.DeleteAsync($"api/reservations/delete/{id}");
    response.EnsureSuccessStatusCode();

    Console.WriteLine("!!! Reservation deleted successfully !!!");
}

async Task UpdateReservation()
{
    await AllReservations();
    char answer = ' ';

    Console.Write("Choose reservation to update by id: ");
    int id = int.Parse(Console.ReadLine());

    response = await client.GetAsync($"api/reservations/details/{id}");
    response.EnsureSuccessStatusCode();
    var reservation = await response.Content.ReadFromJsonAsync<Reservation>();


    Console.Write("Update reservation Name?(y/n): ");
    answer = char.Parse(Console.ReadLine());
    if (answer == 'y')
    {
        Console.Write("Enter reservation name: ");
        reservation.Name = Console.ReadLine();
    }

    Console.Write("Update number of reservation attendees?(y/n): ");
    answer = char.Parse(Console.ReadLine());
    if (answer == 'y')
    {
        Console.Write("Enter reservation attendee number: ");
        reservation.TableFor = int.Parse(Console.ReadLine());
    }

    Console.Write("Update reservation table number?(y/n): ");
    answer = char.Parse(Console.ReadLine());
    if (answer == 'y')
    {
        Console.Write("Enter reservation table number: ");
        reservation.TableNo = int.Parse(Console.ReadLine());
    }

    Console.Write("Update reservation time?(y/n): ");
    answer = char.Parse(Console.ReadLine());
    if (answer == 'y')
    {
        Console.Write("Enter reservation time(yyyy-mm-dd hh:mm PM/AM): ");
        reservation.Time = DateTime.Parse(Console.ReadLine());
    }

    var payload = JsonConvert.SerializeObject(reservation);
    var content = new StringContent(payload, Encoding.UTF8,"application/json");

    response = await client.PutAsync($"api/reservations/update/{id}", content);
    response.EnsureSuccessStatusCode();

    Console.WriteLine("!!! Reservation updated successfully !!!");
}
