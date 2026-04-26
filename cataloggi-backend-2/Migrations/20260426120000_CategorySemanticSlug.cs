using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cataloggi_backend_2.Migrations
{
    /// <inheritdoc />
    public partial class CategorySemanticSlug : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                WITH NameRows AS (
                    SELECT
                        Id,
                        trim(Name) AS BaseName,
                        ROW_NUMBER() OVER (
                            PARTITION BY lower(trim(Name))
                            ORDER BY Id
                        ) AS RowNumber
                    FROM Categories
                )
                UPDATE Categories
                SET Name = (
                    SELECT
                        CASE
                            WHEN RowNumber = 1 THEN BaseName
                            ELSE BaseName || ' ' || RowNumber
                        END
                    FROM NameRows
                    WHERE NameRows.Id = Categories.Id
                );
                """);

            migrationBuilder.Sql("""
                WITH SlugSources AS (
                    SELECT
                        Id,
                        lower(trim(replace(replace(replace(Name, ' ', '-'), '/', '-'), '_', '-'))) AS BaseSlug
                    FROM Categories
                ),
                SlugRows AS (
                    SELECT
                        Id,
                        CASE
                            WHEN BaseSlug = '' THEN 'category'
                            ELSE BaseSlug
                        END AS BaseSlug,
                        ROW_NUMBER() OVER (
                            PARTITION BY CASE
                                WHEN BaseSlug = '' THEN 'category'
                                ELSE BaseSlug
                            END
                            ORDER BY Id
                        ) AS RowNumber
                    FROM SlugSources
                )
                UPDATE Categories
                SET Slug = (
                    SELECT
                        CASE
                            WHEN RowNumber = 1 THEN BaseSlug
                            ELSE BaseSlug || '-' || RowNumber
                        END
                    FROM SlugRows
                    WHERE SlugRows.Id = Categories.Id
                );
                """);

            migrationBuilder.Sql("""
                CREATE UNIQUE INDEX "IX_Categories_Name"
                ON "Categories" ("Name" COLLATE NOCASE);
                """);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Slug",
                table: "Categories",
                column: "Slug",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Categories_Slug",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Categories_Name",
                table: "Categories");
        }
    }
}
