using Newtonsoft.Json;
using System;
using System.Data.SQLite;

namespace ConsoleApplication1
{
    [JsonObject]
    public class Marca
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("fipe_name")]
        public string Nome { get; set; }
        public void Gravar(SQLiteConnection conn, int tipo)
        {
            SQLiteCommand insertSQL = new SQLiteCommand("INSERT INTO TipoMarca VALUES (@id, @tipo, @marca)", conn);
            insertSQL.Parameters.Add(new SQLiteParameter("@id", Id));
            insertSQL.Parameters.Add(new SQLiteParameter("@tipo", tipo));
            insertSQL.Parameters.Add(new SQLiteParameter("@marca", Nome));
            insertSQL.ExecuteNonQuery();
        }
    }
    [JsonObject]
    class Modelo
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        public int IdInt {
            get { return int.Parse(Id); }
        }
        [JsonProperty("fipe_name")]
        public string Nome { get; set; }
        public void Gravar(SQLiteConnection conn, Marca marca)
        {
            SQLiteCommand insertSQL = new SQLiteCommand("INSERT INTO Modelo VALUES (@id, @tipomarca_id, @modelo)", conn);
            insertSQL.Parameters.Add(new SQLiteParameter("@id", IdInt));
            insertSQL.Parameters.Add(new SQLiteParameter("@tipomarca_id", marca.Id));
            insertSQL.Parameters.Add(new SQLiteParameter("@modelo", Nome));
            insertSQL.ExecuteNonQuery();
        }
    }
    [JsonObject]
    class AnoModelo
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
    }
    [JsonObject]
    class Veiculo
    {
        [JsonProperty("preco")]
        public string Preco { get; set; }
        public void Gravar(SQLiteConnection conn, Modelo modelo, AnoModelo ano)
        {
            SQLiteCommand insertSQL = new SQLiteCommand("INSERT INTO AnoModelo(anoModelo, modelo_id, valor) VALUES (@anoModelo, @modelo_id, @valor)", conn);
            insertSQL.Parameters.Add(new SQLiteParameter("@anoModelo", ano.Name));
            insertSQL.Parameters.Add(new SQLiteParameter("@modelo_id", modelo.IdInt));
            insertSQL.Parameters.Add(new SQLiteParameter("@valor", Preco));
            insertSQL.ExecuteNonQuery();
        }
    }
}
