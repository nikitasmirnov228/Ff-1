// See https://aka.ms/new-console-template for more information
using System.Reflection;
using System.Text;

class Person
{
    private string name;
    private string surname;
    private System.DateTime date_of_birth;

    public Person(string n, string sn, System.DateTime dob) { name = n; surname = sn; date_of_birth = dob; }
    public Person() { name = "-"; surname = "-"; date_of_birth = DateTime.Now; }

    public string Name { get { return name; } }
    public string Surname { get { return surname; } }
    public System.DateTime DOB { get { return date_of_birth; } }
    public int IntDoB { get { return Convert.ToInt32(date_of_birth); } set { date_of_birth = Convert.ToDateTime(value); } }

    public override string ToString()
    {
        return "Имя: " + name+ " Фамилия: "+ surname + " Дата рождения: " + date_of_birth.ToString();
    }
    
    public virtual string ToShortString()
    {
        return "Имя: " + name + " Фамилия: " + surname;
    }


}

public enum Education
{
    NONE,
    Specialist,
    Вachelor, 
    SecondEducation
}

class Exam
{
    public string item_name { get; set; }
    public int estimation { get; set; }
    public System.DateTime exam_date { get; set; }

    public Exam(string item, int est, System.DateTime ed) { item_name = item; estimation = est; exam_date = ed; }
    public Exam() { item_name= "-"; estimation = 0; exam_date = DateTime.Now; }

    public override string ToString()
    {
        return item_name +" "+estimation+" "+exam_date;
    }
}

class Student
{
    private Person stud;
    private Education educat;
    private int group;
    private Exam[] exam;

    public Student(Person per, Education edu, int gr, Exam[] ex) {  stud = per; educat = edu; group = gr; exam = ex; }
    

    public Person Stud { get { return stud; } set { } }
    public Education Educat { get { return educat;} set { } }
    public int Group { get { return group; } set { } }
    public Exam[] Exam { get { return exam; } set { } }

    public double Average_Estimation { get => exam.Sum(x => x.estimation) / (double) exam.Count(); }


    public Student(Person studs, Education educats, int groups)
    {
        stud = studs;
        educat = educats;
        group = groups;
        exam = new Exam[] { new Exam() };
    }

    public Student()
    {
        stud = new Person();
        exam = new Exam[] { new Exam() };
    }

    public bool IsThatGrade(Education educatb)
    {
        return educat == educatb;
    }

    public void AddExams(params Exam[] exams)
    {
        if(exams.Length != 0)
        {
            exam = exam.Concat(exams).ToArray();
        }
        
    }

    public override string ToString()
    {
        return string.Join(" ", stud.ToString(), educat, group, string.Join("\nExam: ", (IEnumerable<Exam>)exam));
    }
    public string ToShortString()
    {
        return string.Join(" ", stud.ToString(), educat, group, Average_Estimation);
    }
 
}

class Program
{
    static void Main(string[] args)
    {
        var Person1 = new Person("Jaims", "Bond", DateTime.Now);
        var Person2 = new Person("Ivan", "Ivanov", DateTime.Now);

        var date1 = DateTime.Now;
        var date2 = new DateTime(2004, 11, 30);
        var date3 = new DateTime(2007, 7, 27);

        var student = new Student(Person1, Education.Specialist, 100);
        var student2 = new Student(Person2, Education.Specialist, 100);
        Console.WriteLine(student.ToShortString());

        WriteEnum(typeof(Education));

        student.Educat = Education.Вachelor;
        student.Stud = Person1;
        student.Exam = new Exam[] { new Exam("english", 4, date3) };
        student.Group = 10;

        student.AddExams(new Exam("mathematic", 3, date1), new Exam("PE", 5, date2));

        student2.AddExams(new Exam("geography", 5, date1), new Exam("phisic", 5, date2));

        Console.WriteLine(student2);

        Console.WriteLine(CheckArraysTimes<Student>(1000, 100, nameof(student2.Group), 1000));



        void WriteEnum(Type enumType)
        {
            var names = Enum.GetNames(enumType);
            var values = Enum.GetValues(enumType);

            Console.WriteLine(string.Join("\n", Enumerable.Range(0, values.Length).Select(x => names[x] + " " + ((int[])values)[x])));
        }

        string CheckArraysTimes<T>(int rows, int columns, string nameOfProperty, object valueToSet) where T : new()
        {
            Type objType = typeof(T);
            PropertyInfo propInfo = objType.GetProperty(nameOfProperty);

            var oneDimension = new T[rows * columns].Select(x => new T()).ToArray();
            var twoDimension = new T[rows, columns];
            var ladder = new T[rows].Select(x => new T[columns].Select(x => new T()).ToArray()).ToArray();

            StringBuilder result = new StringBuilder();

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    twoDimension[i, j] = new T();
                }
            }
            DateTime startTime = DateTime.Now;

            for (int i = 0; i < rows * columns; i++)
            {
                propInfo.SetValue(oneDimension[i], valueToSet);
            }

            result.Append(nameof(oneDimension) + " time: " + (DateTime.Now - startTime) + "\n");

            startTime = DateTime.Now;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    propInfo.SetValue(twoDimension[i, j], valueToSet);
                }
            }

            result.Append(nameof(twoDimension) + " time: " + (DateTime.Now - startTime) + "\n");

            startTime = DateTime.Now;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    propInfo.SetValue(ladder[i][j], valueToSet);
                }
            }

            result.Append(nameof(ladder) + " time: " + (DateTime.Now - startTime) + "\n");

            return result.ToString();
        }


    }
}

