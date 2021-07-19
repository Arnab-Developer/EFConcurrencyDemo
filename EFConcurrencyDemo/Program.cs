using EFConcurrencyDemo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;

string conString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=StudentDb;Integrated Security=True";
DbContextOptions<StudentContext> options = new DbContextOptionsBuilder<StudentContext>().UseSqlServer(conString).Options;
StudentContext studentContext = new(options);

StudentModel student = await studentContext.Students.FirstOrDefaultAsync(s => s.Id == 1);
if (student == null) return;
student.FirstName = "jon1";

await studentContext.Database.ExecuteSqlRawAsync("update students set LastName = 'test' where id = 1");

try { await studentContext.SaveChangesAsync(); }
catch (DbUpdateConcurrencyException ex)
{
    foreach (var entry in ex.Entries)
    {
        if (entry.Entity is StudentModel)
        {
            PropertyValues proposedValues = entry.CurrentValues;
            PropertyValues databaseValues = entry.GetDatabaseValues();

            foreach (var property in proposedValues.Properties)
            {
                object proposedValue = proposedValues[property];
                object databaseValue = databaseValues[property];

                switch (property.Name)
                {
                    case nameof(StudentModel.Id):
                        proposedValues[property] = databaseValue;
                        break;
                    case nameof(StudentModel.FirstName):
                        proposedValues[property] = proposedValue;
                        break;
                    case nameof(StudentModel.LastName):
                        proposedValues[property] = databaseValue;
                        break;
                }
            }

            // Refresh original values to bypass next concurrency check
            entry.OriginalValues.SetValues(databaseValues);

            await studentContext.SaveChangesAsync();
        }
        else
        {
            throw new NotSupportedException(
                "Don't know how to handle concurrency conflicts for "
                + entry.Metadata.Name);
        }
    }
}