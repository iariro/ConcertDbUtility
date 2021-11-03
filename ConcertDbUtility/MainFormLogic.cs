using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;

namespace ConcertDbUtility
{
    /// <summary>
    /// �R���T�[�g���DB�o�^�c�[�����C����ʂ̎��������B
    /// </summary>
    partial class MainForm
    {
        /// <summary>
        /// �d���R���T�[�g�`�F�b�N�B
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
                // �z�[����񌟍��B
                int hallId = dataset.HallTable[concert.hallName];

                if (hallId >= 0)
                {
                    // �o�^�ς݂̃z�[���B

                    int concertId =
                        dataset.ConcertTable
                            [concert.date, hallId, concert.kaijou];

                    if (concertId >= 0)
                    {
                        // ���łɑ��݂���R���T�[�g���B

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
                // �G���[�Ȃ��B

                textBoxMessage.AddText("�G���[�Ȃ�");
            }

            textBoxMessage.AddText(string.Empty);

            connection.Close();

            return count <= 0;
        }

        /// <summary>
        /// �󗓃`�F�b�N
        /// </summary>
        private List<string> CheckEmptyValue(List<Concert> concertCollection)
        {
            List<string> errors = new List<string>();

            for (int i = 0; i < concertCollection.Count; i++)
            {
                Concert concert = concertCollection[i];

                if (concert.hallName.Trim().Length <= 0)
                {
                    errors.Add(string.Format("{0}�Ԗڂ̃R���T�[�g�̃z�[�����Ȃ�", i + 1));
                }

                for (int j = 0; j < concert.kyokuCollection.Count; j++)
                {
                    KyokumokuElement kyokumoku = concert.kyokuCollection[j];

                    if (kyokumoku.title.Trim().Length <= 0)
                    {
                        errors.Add(string.Format("{0}�Ԗڂ̃R���T�[�g��{1}�Ԗڂ̋ȖڂȂ�", i + 1, j + 1));
                    }

                    if (kyokumoku.composerName.Trim().Length <= 0)
                    {
                        errors.Add(string.Format("{0}�Ԗڂ̃R���T�[�g��{1}�Ԗڂ̍�Ȏ҂Ȃ�", i + 1, j + 1));
                    }
                }

                for (int j = 0; j < concert.playerCollection.Count; j++)
                {
                    Player player = concert.playerCollection[j];

                    if (player.partName.Trim().Length <= 0)
                    {
                        errors.Add(string.Format("{0}�Ԗڂ̃R���T�[�g��{1}�Ԗڂ̃p�[�g���Ȃ�", i + 1, j + 1));
                    }

                    if (player.playerName.Trim().Length <= 0)
                    {
                        errors.Add(string.Format("{0}�Ԗڂ̃R���T�[�g��{1}�Ԗڂ̉��t�Җ��Ȃ�", i + 1, j + 1));
                    }
                }
            }
            return errors;
        }

        /// <summary>
        /// �����񒷃`�F�b�N
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
                        errors.Add(string.Format("{0}�Ԗڂ̃R���T�[�g�̋Ȗڒ�����", i + 1));
                    }
                }
            }

            return errors;
        }

        /// <summary>
        /// ���͎�����
        /// </summary>
        /// <param name="concertCollection">�R���T�[�g���R���N�V����</param>
        private void InputConcertXml(List<Concert> concertCollection)
        {
            SqlConnection connection =
                new SqlConnection(Properties.Resources.connectionString);
            ConcertDataSet dataset = new ConcertDataSet(connection);
            dataset.Load();

            foreach (Concert concert in concertCollection)
            {
                bool insert = true;

                // �z�[����񌟍��B
                int hallId = dataset.HallTable[concert.hallName];

                if (hallId < 0)
                {
                    // �z�[����񂪂Ȃ��B

                    HallInputForm hallInputForm = new HallInputForm(concert.hallName);

                    if (hallInputForm.ShowDialog() == DialogResult.OK)
                    {
                        // �z�[��������͂����B

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
                            string.Format("�X�L�b�v���܂��� {0} {1} {2}", concert.hallName, concert.date, concert.kaijou));
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

                    // ���t�ғ��́B
                    for (int i = 0; i < concert.playerCollection.Count && insert; i++)
                    {
                        int playerId =
                            dataset.PlayerTable[
                                concert.playerCollection[i].playerName];

                        if (playerId < 0)
                        {
                            // ���t�҂����Ȃ��B

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

                        // �p�[�g�B
                        int partId =
                            dataset.PartTable[
                                concert.playerCollection[i].partName];

                        if (partId < 0)
                        {
                            // �p�[�g���Ȃ��B

                            DialogResult dialogResult =
                                MessageBox.Show(
                                    concert.playerCollection[i].partName +
                                    "��o�^���܂����H",
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

                    // �ȓ��́B
                    for (int i = 0; i < concert.kyokuCollection.Count; i++)
                    {
                        int composerId = dataset.ComposerTable[
                            concert.kyokuCollection[i].composerName];

                        if (composerId < 0)
                        {
                            // ��ȉƂ����Ȃ��B

                            DialogResult dialogResult =
                                MessageBox.Show(
                                    concert.kyokuCollection[i].composerName +
                                    "��o�^���܂����H",
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
                            "{0} {1}��o�^���܂����B\r\n",
                            concert.date.ToShortDateString(),
                            concert.name);
                }
                else
                {
                    textBoxMessage.Text +=
                        concert.name + "���X�L�b�v���܂����B\n";
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
        /// �X�L�[�}�����������B
        /// </summary>
        private void GenerateSchemaFile()
        {
            if (!File.Exists(schemaBaseFilePath))
            {
                // �ǂݍ��ރt�@�C�����Ȃ�

                FileDialog loadDialog = new OpenFileDialog();
                loadDialog.Title = "�X�L�[�}�̂��ƃt�@�C����I�����Ă��������B";
                if (loadDialog.ShowDialog() == DialogResult.OK)
                {
                    // �ǂݍ��ރt�@�C���I���ɂ��āuOK�v�������ꂽ�B

                    FileDialog saveDialog = new SaveFileDialog();
                    saveDialog.Title = "��������t�@�C����I�����Ă��������B";
                    if (saveDialog.ShowDialog() == DialogResult.OK)
                    {
                        // �ۑ�����t�@�C���I���ɂ��āuOK�v�������ꂽ�B

                        schemaBaseFilePath = loadDialog.FileName;
                        schemaFilePath = saveDialog.FileName;
                    }
                }
            }

            // �X�L�[�}�̃x�[�X�ǂݍ��݁B
            ConcertSchemaDocument document = new ConcertSchemaDocument();
            document.Load(schemaBaseFilePath);

            SqlConnection connection =
                new SqlConnection(Properties.Resources.connectionString);
            ConcertDataSet dataset = new ConcertDataSet(connection);
            dataset.Load();

            // ���̗񋓁B
            document.EnumerateAll(dataset);

            document.Save(schemaFilePath);
            textBoxMessage.AddText(schemaFilePath + "�������܂����B");

            connection.Close();
        }
    }
}
