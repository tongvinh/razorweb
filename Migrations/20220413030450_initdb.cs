using System;
using Bogus;
using Microsoft.EntityFrameworkCore.Migrations;
using razorweb.models;

#nullable disable

namespace razorweb.Migrations
{
  public partial class initdb : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.CreateTable(
          name: "articles",
          columns: table => new
          {
            Id = table.Column<int>(type: "int", nullable: false)
                  .Annotation("SqlServer:Identity", "1, 1"),
            Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
            Created = table.Column<DateTime>(type: "datetime2", nullable: false),
            Content = table.Column<string>(type: "ntext", nullable: true)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_articles", x => x.Id);
          });
      //Insert Data
      //Fake Data: Bogus

      Randomizer.Seed = new Random(8675309);

      var fakerAritcle = new Faker<Article>();
      fakerAritcle.RuleFor(x => x.Title, f => f.Lorem.Sentence(5, 5));
      fakerAritcle.RuleFor(x => x.Created, f => f.Date.Between(new DateTime(2022, 1, 1), new DateTime(2022, 7, 30)));
      fakerAritcle.RuleFor(x => x.Content, f => f.Lorem.Paragraphs(1, 4));

      for (int i = 0; i < 150; i++)
      {
        Article article = fakerAritcle.Generate();

        migrationBuilder.InsertData(
            table: "articles",
            columns: new[] { "Title", "Created", "Content" },
            values: new object[] {
                        article.Title,
                        article.Created,
                        article.Content
            }
        );
      }
    }
    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropTable(
          name: "articles");
    }
  }
}
