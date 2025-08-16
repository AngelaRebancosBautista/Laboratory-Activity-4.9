using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratory_Activity_9
{
    namespace CoursePlanner
    {
        class Course
        {
            public string Code { get; set; }
            public string Name { get; set; }
            public List<string> Prerequisites { get; set; }
            public string TimeSlot { get; set; } 

            public Course(string code, string name, List<string> prereqs, string timeSlot)
            {
                Code = code.ToUpper();
                Name = name;
                Prerequisites = prereqs.Select(p => p.ToUpper()).ToList();
                TimeSlot = timeSlot;
            }

            public bool CanEnroll(List<string> completedCourses, out string reason)
            {
                foreach (var prereq in Prerequisites)
                {
                    if (!completedCourses.Contains(prereq))
                    {
                        reason = $"Missing prerequisite: {prereq}";
                        return false;
                    }
                }
                reason = "";
                return true;
            }

            public bool ConflictsWith(Course other)
            {
                return TimeSlot.Equals(other.TimeSlot, StringComparison.OrdinalIgnoreCase);
            }
        }

        class Program
        {
            static void Main(string[] args)
            {
                Console.WriteLine("Course Planner with Prerequisites");

                List<Course> catalog = new List<Course>
            {
                new Course("CS101", "Intro to Programming", new List<string>(), "Mon 9-11"),
                new Course("CS102", "Data Structures", new List<string>{ "CS101" }, "Tue 10-12"),
                new Course("CS103", "Algorithms", new List<string>{ "CS102" }, "Mon 9-11"),
                new Course("MATH101", "Calculus I", new List<string>(), "Wed 8-10"),
                new Course("MATH102", "Calculus II", new List<string>{ "MATH101" }, "Fri 8-10")
            };

                Console.Write("Enter completed courses (comma separated): ");
                List<string> completed = Console.ReadLine()
                    .Split(',', (char)StringSplitOptions.RemoveEmptyEntries)
                    .Select(c => c.Trim().ToUpper())
                    .ToList();

                Console.Write("Enter requested courses (comma separated): ");
                List<string> requested = Console.ReadLine()
                    .Split(',', (char)StringSplitOptions.RemoveEmptyEntries)
                    .Select(c => c.Trim().ToUpper())
                    .ToList();

                List<Course> approved = new List<Course>();

                foreach (var code in requested)
                {
                    var course = catalog.FirstOrDefault(c => c.Code == code);
                    if (course == null)
                    {
                        Console.WriteLine($"{code}: Rejected - Course not found.");
                        continue;
                    }

                    if (!course.CanEnroll(completed, out string reason))
                    {
                        Console.WriteLine($"{course.Code}: Rejected - {reason}");
                        continue;
                    }

                    bool conflict = false;
                    foreach (var app in approved)
                    {
                        if (course.ConflictsWith(app))
                        {
                            Console.WriteLine($"{course.Code}: Rejected - Schedule conflict with {app.Code}");
                            conflict = true;
                            break;
                        }
                    }
                    if (conflict) continue;

                    approved.Add(course);
                }

                Console.WriteLine("\nApproved Courses:");
                foreach (var c in approved)
                {
                    Console.WriteLine($"{c.Code} - {c.Name} ({c.TimeSlot})");
                }
            }
        }
    }
}