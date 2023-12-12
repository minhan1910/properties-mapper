namespace PropertiesHandler
{
    class PersonAddRequest
    {
        public string Name { get; set; }
        public GenderOptions? Gender { get; set; }
    }

    class Person
    {
        public string Name { get; set; }
        public string? Gender { get; set; }
    }

    class PersonResponse
    {
        public string Name { get; set; }
        public string? Gender { get; set; }
    }

    class PersonUpdateRequest
    {
        public string Name { get; set; }
        public GenderOptions? Gender { get; set; }
    }


    internal class Program
    {
        static void Main(string[] args)
        {
            PersonAddRequest personAddRequest = new PersonAddRequest
            {
                Name = "An",
                Gender = GenderOptions.FEMALE
            };

            Console.WriteLine("Person AR: ");
            Console.WriteLine($"GenderType: {personAddRequest.Gender.GetType().Name}");
            Console.WriteLine($"{personAddRequest.Name}, Gender: {personAddRequest.Gender}");
            Console.WriteLine();

            Person? person = PropertiesHandler<PersonAddRequest, Person>.Copy(personAddRequest, new Person());
            Console.WriteLine("Person: ");
            Console.WriteLine($"GenderType: {person?.Gender?.GetType().Name}");
            Console.WriteLine($"{person?.Name}, Gender: {person?.Gender}");
            Console.WriteLine();

            PersonResponse? personResponse = PropertiesHandler<Person, PersonResponse>.Copy(person, new PersonResponse());
            Console.WriteLine("Person Response: ");
            Console.WriteLine($"GenderType: {personResponse?.Gender?.GetType().Name}");
            Console.WriteLine($"{personResponse?.Name}, Gender: {personResponse?.Gender}");
            Console.WriteLine();

            PersonUpdateRequest? personUpdateRequest =
                PropertiesHandler<PersonResponse, PersonUpdateRequest>.Copy(personResponse, new PersonUpdateRequest());
            Console.WriteLine("Person Update Request: ");
            Console.WriteLine($"GenderType: {personUpdateRequest?.Gender?.GetType().Name}");
            Console.WriteLine($"{personUpdateRequest?.Name}, Gender: {personUpdateRequest?.Gender}");
            Console.WriteLine();


            //Person person2 = new Person
            //{
            //    Name = "An2",          
            //    Gender = "FEMALE"
            //};

            //Console.WriteLine("Person AR2: ");
            //Console.WriteLine($"{person2.Name}, Gender: {person2.Gender}");
            //PersonAddRequest? personAddRequest2 = PropertiesHandler<Person, PersonAddRequest>.Copy(person2, new PersonAddRequest());

            //Console.WriteLine("Person AR2: ");
            //Console.WriteLine($"{personAddRequest2?.Name}, Gender: {personAddRequest2?.Gender}");


        }
    }
}