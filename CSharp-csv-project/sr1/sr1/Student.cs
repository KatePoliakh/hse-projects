using System;
using CsvHelper.Configuration;
namespace sr1;

public class Student
{
    //gender,"race/ethnicity","parental level of education","lunch","test preparation course","math score","reading score","writing score"
    public string gender { get; set; }
    public string Raceethic { get; set; }
    public string level { get; set; }
    public string lunch { get; set; }
    public string test { get; set; }
    public int? mathscore { get; set; }
    public int? readingscore { get; set; }
    public int? writingscore { get; set; }
    
    
    public class StudentMapByName : ClassMap<Student>
    {
        public StudentMapByName()
        {
            Map(st => st.gender).Name("gender");
            Map(st => st.Raceethic).Name("race/ethnicity");
            Map(st => st.level).Name("parental level of education");
            Map(st => st.lunch).Name("lunch");
            Map(st => st.test).Name("test preparation course");
            Map(st => st.mathscore).Name("math score");
            Map(st => st.readingscore).Name("reading score");
            Map(st => st.writingscore).Name("writing score");
        }

        
    }
    public override string ToString()
    {
        return $"gender - {gender},race/ethnicity - {Raceethic}, parental level of education - {level}, lunch - {lunch}, test preparation course - {test}, math score - {mathscore}, reading score - {readingscore}, writing score - {writingscore}";
    }

    public string ToStringForRecordFile()
    {
        return $"{gender},{Raceethic},{level},{lunch},{test},{mathscore},{readingscore},{writingscore}";
    }
}