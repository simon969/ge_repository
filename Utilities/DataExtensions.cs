using System;
using System.IO;
using System.Data;
using System.Text;


// https://stackoverflow.com/questions/29672383/streaming-data-from-a-nvarcharmax-column-using-c-sharp

public static class DataExtensions
{
    public static Stream GetBinaryStream(this IDataRecord record, int ordinal=0)
    {
        return new DbBinaryFieldStream(record, ordinal);
    }

    public static Stream GetBinaryStream(this IDataRecord record, string name)
    {
        int i = record.GetOrdinal(name);
        return GetBinaryStream(record, i);
    }
    public static Stream GetCharStream(this IDataRecord record, int ordinal=0, Encoding encode=null)
    {
        if (encode == null) encode = Encoding.Unicode;
        return new DbCharFieldStream(record, ordinal, encode);
    }

    public static Stream GetCharStream(this IDataRecord record, string name, Encoding encode=null )
    {
        int i = record.GetOrdinal(name);
        return GetCharStream(record, i, encode);
    }

    private class DbCharFieldStream : Stream
    {
        private readonly IDataRecord _record;
        private readonly int _fieldIndex;
        private long _position;
        private long _length = -1;
        private Encoding _encode;
        
        public DbCharFieldStream(IDataRecord record, int fieldIndex, Encoding encode)
        {
            _encode = encode;
            _record = record;
            _fieldIndex = fieldIndex;
        }

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanSeek
        {
            get { return true; }
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override void Flush()
        {
            throw new NotSupportedException();
        }

        public override long Length
        {
            get
            {
                if (_length < 0)
                {
                    _length = _record.GetBytes(_fieldIndex, 0, null, 0, 0);
                }
                return _length;
            }
        }

        public override long Position
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (_encode==Encoding.UTF8) return Read_UTF8(buffer, offset, count);
            if (_encode==Encoding.ASCII) return Read_ASCII(buffer, offset, count);
            if (_encode==Encoding.Unicode) return Read_Unicode(buffer, offset, count);
            return 0;
        }

        private int Read_UTF8(byte[] buffer, int offset, int count)
        {
            char[] cbuffer = Encoding.UTF8.GetChars(buffer);
            long nRead = _record.GetChars(_fieldIndex, _position, cbuffer, offset, count); 
            byte[] lbuffer = Encoding.UTF8.GetBytes(cbuffer);
            lbuffer.CopyTo(buffer,0);
            _position += nRead;
            return (int)nRead;
        }
        private int Read_ASCII(byte[] buffer, int offset, int count)
        {
            char[] cbuffer = Encoding.ASCII.GetChars(buffer);
            long nRead = _record.GetChars(_fieldIndex, _position, cbuffer, offset, count); 
            byte[] lbuffer = Encoding.ASCII.GetBytes(cbuffer);
            lbuffer.CopyTo(buffer,0);
            _position += nRead;
            return (int)nRead;
        }
        private int Read_Unicode(byte[] buffer, int offset, int count)
        {
            char[] cbuffer = Encoding.Unicode.GetChars(buffer);
            long nRead = _record.GetChars(_fieldIndex, _position, cbuffer, offset, count / 2); 
            byte[] lbuffer = Encoding.Unicode.GetBytes(cbuffer);
            lbuffer.CopyTo(buffer,0);
            _position += nRead;
            return (int)nRead;
        }
        public override long Seek(long offset, SeekOrigin origin)
        {
            long newPosition = _position;
            switch (origin)
            {
                case SeekOrigin.Begin:
                    newPosition = offset;
                    break;
                case SeekOrigin.Current:
                    newPosition = _position + offset;
                    break;
                case SeekOrigin.End:
                    newPosition = this.Length - offset;
                    break;
                default:
                    break;
            }
            if (newPosition < 0)
                throw new ArgumentOutOfRangeException("offset");
            _position = newPosition;
            return _position;
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }
    }

    private class DbBinaryFieldStream : Stream
    {
        private readonly IDataRecord _record;
        private readonly int _fieldIndex;
        private long _position;
        private long _length = -1;

        public DbBinaryFieldStream(IDataRecord record, int fieldIndex)
        {
            _record = record;
            _fieldIndex = fieldIndex;
        }

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanSeek
        {
            get { return true; }
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override void Flush()
        {
            throw new NotSupportedException();
        }

        public override long Length
        {
            get
            {
                if (_length < 0)
                {
                    _length = _record.GetBytes(_fieldIndex, 0, null, 0, 0);
                }
                return _length;
            }
        }

        public override long Position
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            long nRead = _record.GetBytes(_fieldIndex, _position, buffer, offset, count);
            _position += nRead;
            return (int) nRead;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            long newPosition = _position;
            switch (origin)
            {
                case SeekOrigin.Begin:
                    newPosition = offset;
                    break;
                case SeekOrigin.Current:
                    newPosition = _position + offset;
                    break;
                case SeekOrigin.End:
                    newPosition = this.Length - offset;
                    break;
                default:
                    break;
            }
            if (newPosition < 0)
                throw new ArgumentOutOfRangeException("offset");
            _position = newPosition;
            return _position;
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }
    }
}