using System;
using System.Collections.Generic;
using System.Linq;

class Vehicle
{
    public string Plate { get; set; }
    public string Color { get; set; }
    public string Type { get; set; } // Mobil / Motor
}

class Slot
{
    public int Number { get; set; }
    public Vehicle Vehicle { get; set; }
    public bool IsOccupied => Vehicle != null;
}

class ParkingSystem
{
    private List<Slot> slots = new List<Slot>();

    public void CreateParkingLot(int count)
    {
        slots = Enumerable.Range(1, count).Select(i => new Slot { Number = i }).ToList();
        Console.WriteLine($"Created a parking lot with {count} slots");
    }

    public void Park(string plate, string color, string type)
    {
        var slot = slots.FirstOrDefault(s => !s.IsOccupied);
        if (slot == null)
        {
            Console.WriteLine("Sorry, parking lot is full");
            return;
        }

        slot.Vehicle = new Vehicle
        {
            Plate = plate,
            Color = color,
            Type = type
        };

        Console.WriteLine($"Allocated slot number: {slot.Number}");
    }

    public void Leave(int slotNumber)
    {
        var slot = slots.FirstOrDefault(s => s.Number == slotNumber);
        if (slot != null && slot.IsOccupied)
        {
            slot.Vehicle = null;
            Console.WriteLine($"Slot number {slotNumber} is free");
        }
    }

    public void Status()
    {
        Console.WriteLine("Slot No.    Type       Registration No    Colour");
        foreach (var s in slots.Where(s => s.IsOccupied))
        {
            Console.WriteLine($"{s.Number,-10}{s.Vehicle.Type,-11}{s.Vehicle.Plate,-20}{s.Vehicle.Color}");
        }
    }

    public void TypeOfVehicles(string type)
    {
        int count = slots.Count(s => s.IsOccupied && s.Vehicle.Type.Equals(type, StringComparison.OrdinalIgnoreCase));
        Console.WriteLine(count);
    }

    public void RegistrationNumbersByPlateType(bool isOdd)
    {
        var result = slots
            .Where(s => s.IsOccupied)
            .Select(s => s.Vehicle)
            .Where(v =>
            {
                var digits = new string(v.Plate.Where(char.IsDigit).ToArray());
                if (int.TryParse(digits, out int number))
                    return isOdd ? number % 2 == 1 : number % 2 == 0;
                return false;
            })
            .Select(v => v.Plate);

        Console.WriteLine(string.Join(", ", result));
    }

    public void RegistrationNumbersByColor(string color)
    {
        var result = slots
            .Where(s => s.IsOccupied && s.Vehicle.Color.Equals(color, StringComparison.OrdinalIgnoreCase))
            .Select(s => s.Vehicle.Plate);

        Console.WriteLine(string.Join(", ", result));
    }

    public void SlotNumbersByColor(string color)
    {
        var result = slots
            .Where(s => s.IsOccupied && s.Vehicle.Color.Equals(color, StringComparison.OrdinalIgnoreCase))
            .Select(s => s.Number);

        Console.WriteLine(string.Join(", ", result));
    }

    public void SlotNumberByPlate(string plate)
    {
        var slot = slots.FirstOrDefault(s => s.IsOccupied && s.Vehicle.Plate.Equals(plate, StringComparison.OrdinalIgnoreCase));
        if (slot != null)
            Console.WriteLine(slot.Number);
        else
            Console.WriteLine("Not found");
    }
}

class Program
{
    static void Main()
    {
        var system = new ParkingSystem();

        while (true)
        {
            var input = Console.ReadLine();
            if (input == null) continue;
            var parts = input.Split(' ');
            var command = parts[0].ToLower();

            switch (command)
            {
                case "create_parking_lot":
                    int count = int.Parse(parts[1]);
                    system.CreateParkingLot(count);
                    break;

                case "park":
                    string plate = parts[1];
                    string color = parts[2];
                    string type = parts[3];
                    system.Park(plate, color, type);
                    break;

                case "leave":
                    int slotNumber = int.Parse(parts[1]);
                    system.Leave(slotNumber);
                    break;

                case "status":
                    system.Status();
                    break;

                case "type_of_vehicles":
                    system.TypeOfVehicles(parts[1]);
                    break;

                case "registration_numbers_for_vehicles_with_ood_plate":
                    system.RegistrationNumbersByPlateType(isOdd: true);
                    break;

                case "registration_numbers_for_vehicles_with_event_plate":
                    system.RegistrationNumbersByPlateType(isOdd: false);
                    break;

                case "registration_numbers_for_vehicles_with_colour":
                    system.RegistrationNumbersByColor(parts[1]);
                    break;

                case "slot_numbers_for_vehicles_with_colour":
                    system.SlotNumbersByColor(parts[1]);
                    break;

                case "slot_number_for_registration_number":
                    system.SlotNumberByPlate(parts[1]);
                    break;

                case "exit":
                    return;

                default:
                    Console.WriteLine("Perintah tidak dikenali.");
                    break;
            }
        }
    }
}
