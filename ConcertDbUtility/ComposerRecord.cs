using System;
using System.Collections.Generic;
using System.Text;

namespace ConcertDbUtility
{
	/// <summary>
	/// ��ȉƏ��
	/// </summary>
	class ComposerRecord
	{
		readonly public string name;
        readonly public int namevalue;

		/// <summary>
		/// �w��̒l�������o�[�Ɋ��蓖�Ă�
		/// </summary>
		/// <param name="name">��ȉƖ�</param>
        /// <param name="namevalue">�m���x</param>
        public ComposerRecord(string name, int namevalue)
		{
			this.name = name;
            this.namevalue = namevalue;
		}
	}
}
