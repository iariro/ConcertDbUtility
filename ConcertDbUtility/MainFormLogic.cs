using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;

namespace ConcertDbUtility
{
    /// <summary>
    /// コンサート情報DB登録ツールメイン画面の実処理部。
    /// </summary>
    partial class MainForm
    {
        /// <summary>
        /// 重複コンサートチェック。
        /// </summary>
        private bool CheckDuplicateConcert(List<Concert> concertCollection)
        {
            SqlConnection connection =
                new SqlConnection(Properties.Resources.connectionString);
            ConcertDataSet dataset = new ConcertDataSet(connection);
            dataset.Load();

            int count = 0;
            foreach (Concert concert in concertCollection)
            {
                // ホール情報検索。
                int hallId = dataset.HallTable[concert.hallName];

                if (hallId >= 0)
                {
                    // 登録済みのホール。

                    int concertId =
                        dataset.ConcertTable
                            [concert.date, hallId, concert.kaijou];

                    if (concertId >= 0)
                    {
                        // すでに存在するコンサート情報。

                        string message =
                            string.Format(
                                "{0} {1} {2}",
                                concertId,
                                concert.date,
                                concert.hallName);

                        textBoxMessage.AddText(message);

                        count++;
                    }
                }
            }

            if (count <= 0)
            {
                // エラーなし。

                textBoxMessage.AddText("エラーなし");
            }

            textBoxMessage.AddText(string.Empty);

            connection.Close();

            return count <= 0;
        }

        /// <summary>
        /// 空欄チェック
        /// </summary>
        private List<string> CheckEmptyValue(List<Concert> concertCollection)
        {
            List<string> errors = new List<string>();

            for (int i = 0; i < concertCollection.Count; i++)
            {
                Concert concert = concertCollection[i];

                if (concert.hallName.Trim().Length <= 0)
                {
                    errors.Add(string.Format("{0}番目のコンサートのホール名なし", i + 1));
                }

                for (int j = 0; j < concert.kyokuCollection.Count; j++)
                {
                    KyokumokuElement kyokumoku = concert.kyokuCollection[j];

                    if (kyokumoku.title.Trim().Length <= 0)
                    {
                        errors.Add(string.Format("{0}番目のコンサートの{1}番目の曲目なし", i + 1, j + 1));
                    }

                    if (kyokumoku.composerName.Trim().Length <= 0)
                    {
                        errors.Add(string.Format("{0}番目のコンサートの{1}番目の作曲者なし", i + 1, j + 1));
                    }
                }

                for (int j = 0; j < concert.playerCollection.Count; j++)
                {
                    Player player = concert.playerCollection[j];

                    if (player.partName.Trim().Length <= 0)
                    {
                        errors.Add(string.Format("{0}番目のコンサートの{1}番目のパート名なし", i + 1, j + 1));
                    }

                    if (player.playerName.Trim().Length <= 0)
                    {
                        errors.Add(string.Format("{0}番目のコンサートの{1}番目の演奏者名なし", i + 1, j + 1));
                    }
                }
            }
            return errors;
        }

        /// <summary>
        /// 文字列長チェック
        /// </summary>
        private List<string> CheckStringLength(List<Concert> concertCollection)
        {
            List<string> errors = new List<string>();

            for (int i = 0; i < concertCollection.Count; i++)
            {
                Concert concert = concertCollection[i];
                foreach (KyokumokuElement kyokumoku in concert.kyokuCollection)
                {
                    if (kyokumoku.title.Length > 100)
                    {
                        errors.Add(string.Format("{0}番目のコンサートの曲目長すぎ", i + 1));
                    }
                }
            }

            return errors;
        }

        /// <summary>
        /// 入力実処理
        /// </summary>
        /// <param name="concertCollection">コンサート情報コレクション</param>
        private void InputConcertXml(List<Concert> concertCollection)
        {
            SqlConnection connection =
                new SqlConnection(Properties.Resources.connectionString);
            ConcertDataSet dataset = new ConcertDataSet(connection);
            dataset.Load();

            foreach (Concert concert in concertCollection)
            {
                bool insert = true;

                // ホール情報検索。
                int hallId = dataset.HallTable[concert.hallName];

                if (hallId < 0)
                {
                    // ホール情報がない。

                    HallInputForm hallInputForm = new HallInputForm(concert.hallName);

                    if (hallInputForm.ShowDialog() == DialogResult.OK)
                    {
                        // ホール情報を入力した。

                        dataset.HallTable.Add(
                            new HallRecord(
                                concert.hallName,
                                hallInputForm.HallAddress));

                        dataset.HallTable.Save();
                        dataset.HallTable.Load(dataset);

                        hallId = dataset.HallTable[concert.hallName];
                    }
                    else
                    {
                        insert = false;
                    }
                }

                if (insert)
                {
                    if (dataset.ConcertTable[concert.date, hallId, concert.kaijou] >= 0)
                    {
                        textBoxMessage.AddText(
                            string.Format("スキップしました {0} {1} {2}", concert.hallName, concert.date, concert.kaijou));
                        continue;
                    }

                    DateTime createdate = DateTime.Now;
                    ConcertRecord record =
                        new ConcertRecord(
                            concert.name,
                            concert.date,
                            concert.kaijou,
                            concert.kaien,
                            hallId,
                            concert.ryoukin,
                            createdate);

                    dataset.ConcertTable.Add(record);
                    dataset.ConcertTable.Save();
                    dataset.ConcertTable.Load(dataset);

                    int concertId =
                        dataset.ConcertTable
                            [concert.date, hallId, concert.kaijou];

                    // 演奏者入力。
                    for (int i = 0; i < concert.playerCollection.Count && insert; i++)
                    {
                        int playerId =
                            dataset.PlayerTable[
                                concert.playerCollection[i].playerName];

                        if (playerId < 0)
                        {
                            // 演奏者がいない。

                            PlayerInputForm playerInformationInputForm =
                                new PlayerInputForm
                                    (concert.playerCollection[i].
                                        playerName);

                            if (playerInformationInputForm.ShowDialog() ==
                                DialogResult.OK)
                            {
                                dataset.PlayerTable.Add(
                                    new PlayerRecord(
                                        concert.playerCollection[i].
                                            playerName,
                                        playerInformationInputForm.
                                            SiteUrl,
                                            true));

                                dataset.PlayerTable.Save();
                                dataset.PlayerTable.Load(dataset);

                                playerId = dataset.PlayerTable[
                                    concert.playerCollection[i].playerName];
                            }
                            else
                            {
                                insert = false;
                            }
                        }

                        // パート。
                        int partId =
                            dataset.PartTable[
                                concert.playerCollection[i].partName];

                        if (partId < 0)
                        {
                            // パートがない。

                            DialogResult dialogResult =
                                MessageBox.Show(
                                    concert.playerCollection[i].partName +
                                    "を登録しますか？",
                                    "caption",
                                    MessageBoxButtons.OKCancel);

                            if (dialogResult == DialogResult.OK)
                            {
                                dataset.PartTable.Add(
                                    new PartRecord(
                                        concert.playerCollection[i].
                                            partName));

                                dataset.PartTable.Save();
                                dataset.PartTable.Load(dataset);

                                partId =
                                    dataset.PartTable[
                                        concert.playerCollection[i].
                                            partName];
                            }
                            else
                            {
                                insert = false;
                            }
                        }

                        if (insert)
                        {
                            dataset.ShutsuenTable.Add(
                                new ShutsuenRecord
                                    (concertId, playerId, partId));
                        }
                    }

                    // 曲入力。
                    for (int i = 0; i < concert.kyokuCollection.Count; i++)
                    {
                        int composerId = dataset.ComposerTable[
                            concert.kyokuCollection[i].composerName];

                        if (composerId < 0)
                        {
                            // 作曲家がいない。

                            DialogResult dialogResult =
                                MessageBox.Show(
                                    concert.kyokuCollection[i].composerName +
                                    "を登録しますか？",
                                    "caption",
                                    MessageBoxButtons.OKCancel);

                            if (dialogResult == DialogResult.OK)
                            {
                                dataset.ComposerTable.Add(
                                    new ComposerRecord(
                                        concert.kyokuCollection[i].composerName,
                                        3));

                                dataset.ComposerTable.Save();
                                dataset.ComposerTable.Load(dataset);

                                composerId =
                                    dataset.ComposerTable[
                                        concert.kyokuCollection[i].
                                            composerName];
                            }
                            else
                            {
                                insert = false;
                            }
                        }

                        if (insert)
                        {
                            dataset.KyokumokuTable.Add(
                                new KyokumokuRecord(
                                    concertId,
                                    composerId,
                                    concert.kyokuCollection[i].title.Trim()));
                        }
                    }

                    textBoxMessage.Text +=
                        string.Format(
                            "{0} {1}を登録しました。\r\n",
                            concert.date.ToShortDateString(),
                            concert.name);
                }
                else
                {
                    textBoxMessage.Text +=
                        concert.name + "をスキップしました。\n";
                }
            }

            dataset.HallTable.Save();
            dataset.PlayerTable.Save();
            dataset.PartTable.Save();
            dataset.ComposerTable.Save();
            dataset.ShutsuenTable.Save();
            dataset.KyokumokuTable.Save();

            connection.Close();

            GenerateSchemaFile();
        }

        /// <summary>
        /// スキーマ生成実処理。
        /// </summary>
        private void GenerateSchemaFile()
        {
            if (!File.Exists(schemaBaseFilePath))
            {
                // 読み込むファイルがない

                FileDialog loadDialog = new OpenFileDialog();
                loadDialog.Title = "スキーマのもとファイルを選択してください。";
                if (loadDialog.ShowDialog() == DialogResult.OK)
                {
                    // 読み込むファイル選択について「OK」が押された。

                    FileDialog saveDialog = new SaveFileDialog();
                    saveDialog.Title = "生成するファイルを選択してください。";
                    if (saveDialog.ShowDialog() == DialogResult.OK)
                    {
                        // 保存するファイル選択について「OK」が押された。

                        schemaBaseFilePath = loadDialog.FileName;
                        schemaFilePath = saveDialog.FileName;
                    }
                }
            }

            // スキーマのベース読み込み。
            ConcertSchemaDocument document = new ConcertSchemaDocument();
            document.Load(schemaBaseFilePath);

            SqlConnection connection =
                new SqlConnection(Properties.Resources.connectionString);
            ConcertDataSet dataset = new ConcertDataSet(connection);
            dataset.Load();

            // 候補の列挙。
            document.EnumerateAll(dataset);

            document.Save(schemaFilePath);
            textBoxMessage.AddText(schemaFilePath + "生成しました。");

            connection.Close();
        }
    }
}
