using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDApplication.Process
{
    class WebviewClass
    {
    }

    // Reading of response content stream happens asynchronously, and WebView2 does not 
    // directly dispose the stream once it read.  Therefore, use the following stream
    // class, which properly disposes when WebView2 has read all data.  For details, see
    // [CoreWebView2 does not close stream content](https://github.com/MicrosoftEdge/WebView2Feedback/issues/2513).
    class ManagedStream : Stream
    {
        public ManagedStream(Stream s)
        {
            s_ = s;
        }

        public override bool CanRead
        {
            get
            {
                return s_.CanRead;
            }
        }

        public override bool CanSeek
        {
            get
            {
                return s_.CanSeek;
            }
        }

        public override bool CanWrite
        {
            get
            {
                return s_.CanWrite;
            }
        }

        public override long Length
        {
            get
            {
                return s_.Length;
            }
        }

        public override long Position
        {
            get
            {
                return s_.Position;
            }
            set
            {
                s_.Position = value;
            }
        }

        public override void Flush()
        {
            throw new NotImplementedException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return s_.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int read = 0;
            try
            {
                read = s_.Read(buffer, offset, count);
                if (read == 0)
                {
                    s_.Dispose();
                }
            }
            catch
            {
                s_.Dispose();
                throw;
            }
            return read;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        private Stream s_;
    }
}
