using Microsoft.Data.Sqlite;

namespace Movies_app.Resources.Data_Source
{
    /// <summary>
    /// Classe para manipulação de banco de dados
    /// </summary>
    public class Crud
    {
        /// <summary>
        /// Insere dados no banco de dados
        /// </summary>
        /// <param name="conn">Conexão com o banco de dados</param>
        /// <param name="table">Tabela cuja qual se quer adicionar os dados</param>
        /// <param name="args">Valores a serem inseridos na tabela</param>
        /// <returns>[true] - Se os dados foram inseridos corretamente || [false] - Em caso de erro ao inserir os dados</returns>
        public bool Create(SqliteConnection conn, string table, params object[] args)
        {
            int id = GetMaxId(conn, table);

            try
            {
                if (id == -1) throw new Exception();

                if (conn.State == System.Data.ConnectionState.Closed) conn.Open();

                string qry = string.Format("INSERT INTO {0} VALUES ({1}, ", table, id);

                for (int i = 0; i < args.Length; i++)
                {
                    qry += "'" + args[i] + "', ";
                }

                qry = qry.Substring(0, qry.Length - 2) + ")";

                using (var cmd = new SqliteCommand(qry, conn))
                {
                    int rows = cmd.ExecuteNonQuery();

                    if (rows == 0)
                    {
                        throw new Exception();
                    }

                    conn.Close();
                    return true;
                }
            }

            catch
            {
                if (conn.State == System.Data.ConnectionState.Open) conn.Close();

                return false;
            }
        }

        /// <summary>
        /// Realiza uma consulta na base de dados
        /// </summary>
        /// <param name="conn">Conexão com o banco de dados</param>
        /// <param name="table">Tabela cuja qual se deseja obter os dados</param>
        /// <param name="args">[Posições pares] - Nome da coluna, [Posições ímpares] - Valor da coluna</param>
        /// <returns>Uma matriz contendo os valores das colunas de cada linha compatível com os parâmetros || [null] - Em caso de erro</returns>
        public object[][]? Read(SqliteConnection conn, string table, params object[] args)
        {
            string qry = string.Format("SELECT * FROM {0} WHERE", table);

            for (int i = 0; i < args.Length; i++)
            {
                if (i % 2 == 0)
                {
                    qry += " " + args[i] + " = ";
                }

                else
                {
                    qry += "'" + args[i] + "' AND";
                }
            }

            qry = qry[..^4];

            try
            {
                if (conn.State == System.Data.ConnectionState.Closed) conn.Open();

                using (var cmd = new SqliteCommand(qry, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        object[][] aux = Array.Empty<object[]>();
                        var j = 0;

                        while (reader.Read())
                        {
                            aux = aux.Append(Array.Empty<object>()).ToArray();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                aux[j] = aux[j].Append(reader[i]).ToArray();
                            }

                            j++;
                        }

                        if (aux.Length == 0)
                        {
                            throw new Exception();
                        }

                        conn.Close();
                        return aux;
                    }
                }
            }

            catch
            {
                if (conn.State == System.Data.ConnectionState.Open) conn.Close();
                return null;
            }
        }

        /// <summary>
        /// Atualiza valores da tabela informada
        /// </summary>
        /// <param name="conn">Conexão com o banco de dados</param>
        /// <param name="table">Tabela a ser alterada</param>
        /// <param name="id">Id do elemento a ser alterado</param>
        /// <param name="args">[Posições pares] - Nome da coluna, [Posições ímpares] - Valor da coluna</param>
        /// <returns>[true] - Se a atualização ocorrer com sucesso || [false] - Caso ocorra algum erro</returns>
        public bool Update(SqliteConnection conn, string table, int id, params object[] args)
        {
            string qry = string.Format("UPDATE {0} SET", table);

            for (int i = 0; i < args.Length; i++)
            {
                if (i % 2 == 0)
                {
                    qry += " " + args[i] + " = ";
                }

                else
                {
                    qry += "'" + args[i] + "',";
                }
            }

            qry = qry[..^1];
            qry += " WHERE id = " + id;

            try
            {
                if (conn.State == System.Data.ConnectionState.Closed) conn.Open();

                using (SqliteCommand cmd = new(qry, conn))
                {
                    var result = cmd.ExecuteNonQuery();
                    conn.Close();
                    return result > 0;
                }
            }

            catch (Exception e)
            {
                if (conn.State == System.Data.ConnectionState.Open) conn.Close();
                return false;
            }
        }

        /// <summary>
        /// Exclui dados da tabela informada
        /// </summary>
        /// <param name="conn">Conexão com o banco de dados</param>
        /// <param name="table">Tabela a ser modificada</param>
        /// <param name="id">Id do eemento a ser excluído</param>
        /// <returns>[true] - Se os dados forem excluídos com sucesso || [false] - Caso ocorra algum erro</returns>
        public bool Delete(SqliteConnection conn, string table, int id)
        {
            string qry = string.Format("DELETE FROM {0} WHERE id = {1}", table, id);

            try
            {
                if (conn.State == System.Data.ConnectionState.Closed) conn.Open();

                using(SqliteCommand cmd = new(qry, conn))
                {
                    var result = cmd.ExecuteNonQuery();
                    conn.Close();
                    return result > 0;
                }
            }

            catch
            {
                if (conn.State == System.Data.ConnectionState.Open) conn.Close();
                return false;
            }
        }

        /// <summary>
        /// Obtém o próximo valor de id válido para a tabela informada
        /// </summary>
        /// <param name="conn">Conexão com a base de dados</param>
        /// <param name="table">Tabela da base de dados cuja qual se deseja obter um id válido</param>
        /// <returns>[int] - Próximo id válido || [-1] - Em caso de erro ao obter o id</returns>
        private int GetMaxId(SqliteConnection conn, string table)
        {
            string qry = string.Format("SELECT MAX(id) FROM {0}", table);

            try
            {
                if (conn.State == System.Data.ConnectionState.Closed) conn.Open();

                using (var cmd = new SqliteCommand(qry, conn))
                {
                    var result = cmd.ExecuteScalar() ?? throw new Exception();
                    conn.Close();
                    return (result is DBNull) ? 1 : Convert.ToInt32(result) + 1;
                }
            }

            catch
            {
                if (conn.State == System.Data.ConnectionState.Open) conn.Close();
                return -1;
            }
        }
    }
}