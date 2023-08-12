using System;
using System.Globalization;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;


namespace sr1
{
    
     internal class Program
     {
         //Validate File name
         public static bool FileNameIsValid (string filename)
         {
             var pattern = Path.GetInvalidFileNameChars();
             return !filename.Any(pattern.Contains);
         }
         private static List<Student> _students; //Поле объектов Student 
         
         //Read csv-file and return list of objects Student
        public static List<Student> ReadFile (string fileName)
        {
            var configuration = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = "," 
            };
            List<Student> data;
            using (var fs = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (var textReader = new StreamReader(fs))
                using (var csv = new CsvReader(textReader, configuration))
                {
                    csv.Context.RegisterClassMap<Student.StudentMapByName>();
                    data = csv.GetRecords<Student>().ToList();
                }
            }
            return data;
        }
        
        //Record list of objects "Student" to new file
        public static void RecordCsvFIle(List<Student> data, string path)
        {
            using (var streamWriter = new StreamWriter(path))
            using (var csv = new CsvWriter(streamWriter, CultureInfo.InvariantCulture))
            {
                csv.Context.RegisterClassMap<Student.StudentMapByName>();
                csv.WriteRecords(data);
            } 
        }

        //2nd task - students with property test preparation course = completed
        //write and record new file 
        public static void TestCompleted()
        {
            var studentsCompleted = _students.Where(student => student.test == "completed").ToList();
            RecordCsvFIle(studentsCompleted, "Test_Preparation.csv");
            foreach (var el in studentsCompleted)
                Console.WriteLine(el);
        }
        //3rd task - Group students by type of lunch ans sort by math score with differences max and min values
        //write and record new file
        public static void SortedByLunchAndMathScore()
        {
            var groupbylunch = _students.GroupBy(student => student.lunch).ToDictionary(x => x.Key, x => x.ToList());
            var max_mathscore_lunch_standard = groupbylunch["standard"].Max(student => student.mathscore);
            var min_mathscore_lunch_standard = groupbylunch["standard"].Min(student => student.mathscore);
            Console.WriteLine($"Lunch - Standard - Difference of max and min math score: {max_mathscore_lunch_standard - min_mathscore_lunch_standard}");
            var max_mathscore_lunch_freereduced = groupbylunch["free/reduced"].Max(student => student.mathscore);
            var min_mathscore_lunch_freereduced = groupbylunch["free/reduced"].Min(student => student.mathscore);
            Console.WriteLine($"Lunch - Free/Reduced - Difference of max and min math score: {max_mathscore_lunch_freereduced - min_mathscore_lunch_freereduced}");

            foreach (var kvp in groupbylunch)
            {
                groupbylunch[kvp.Key] = kvp.Value.OrderBy(student => student.mathscore).ToList();
                Console.WriteLine(kvp.Key);
                kvp.Value.ForEach(x => Console.WriteLine(x));
            }
            RecordCsvFIle(_students.OrderBy(student => student.lunch).ThenBy(student => student.mathscore).ToList(), "Sorted_Students.csv"); 

        }

        //4th task - Average value of all exams in group of all female students 
        //write and record new file
        public static void Female_and_AverageScore(string PathFromUser)
        {
            var studentsfemale = _students.Where(student => student.gender == "female").ToList();

            foreach (var el in studentsfemale)
            {
                Console.WriteLine($"{el}, averagescore - {el.mathscore + el.readingscore + el.writingscore/3} ");
            }
            RecordCsvFIle(studentsfemale, PathFromUser);
        }
        //5th task - Statistics 
        public static void Statistics()
        {
            Console.WriteLine("Statistics: ");
            Console.WriteLine($"1 - Count of students: {_students.Count} ");
            var groupbyrace = _students.GroupBy(student => student.Raceethic).ToDictionary(x => x.Key, x => x.ToList());
            Console.WriteLine($"2 - Count of different races/ethnicities: {_students.GroupBy(student => student.Raceethic).Count()}  ");
            foreach (var kpv in groupbyrace)
            {
                Console.WriteLine($"  - Count of students in {kpv.Key} group: {kpv.Value.Count}");
            }

            var mathscore = _students.Where(student => student.mathscore > 50).Count();
            Console.WriteLine($"3 - Statictics about exams:  ");
            Console.WriteLine($"  - Math: {_students.Where(student => student.mathscore > 50).Count()}");
            Console.WriteLine($"  - Reading: {_students.Where(student => student.readingscore > 50).Count()}");
            Console.WriteLine($"  - Writing: {_students.Where(student => student.writingscore > 50).Count()}");
            
        }

        public static void Main()
        {
            try
            {
                int CommandfromUser = -1;
                Console.WriteLine("Analysis of student performance in exams");
                Console.WriteLine("Enter path of the file:");
                string path = Console.ReadLine();
                if (!FileNameIsValid(path))
                    Console.WriteLine("Incorrect FileName!");
                else
                {
                    _students = ReadFile(path);
                    do
                    {
                        Console.WriteLine("======================================MENU==========================================");
                        Console.WriteLine("1 - Get Test_Preparation.csv, where property test preparation course = completed");
                        Console.WriteLine("2 - Get Sorted_Students.csv, where students sorted by lunch and math score");
                        Console.WriteLine("3 - Get new file with all female students");
                        Console.WriteLine("4 - Statistics");
                        Console.WriteLine("5 - Close");
                        Console.WriteLine("====================================================================================");
                        Console.WriteLine("Enter number of command: ");
                        if (!int.TryParse(Console.ReadLine(), out CommandfromUser))
                        {
                            Console.WriteLine("Incorrect input!");
                        }
                        else
                        {
                            if (CommandfromUser == 1)
                            {
                                TestCompleted();
                            }

                            if (CommandfromUser == 2)
                            {
                                SortedByLunchAndMathScore();
                            }

                            if (CommandfromUser == 3)
                            {
                                Console.WriteLine("Enter name of new File: ");
                                string PathFromUser = Console.ReadLine();
                                if (!FileNameIsValid(PathFromUser))
                                    Console.WriteLine("Incorrect FileName!");
                                else 
                                    Female_and_AverageScore(PathFromUser);
                            }

                            if (CommandfromUser == 4)
                            {
                                Statistics();
                            }

                        }
                    } while (CommandfromUser != 5);
                }

            }
            catch (CsvHelperException)
            {
                Console.WriteLine("Incorrect file format!");
                throw;
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("File not Found!");
            }
            

        }
    }
}