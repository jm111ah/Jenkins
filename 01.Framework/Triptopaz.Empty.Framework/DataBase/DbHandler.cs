using System;
using System.Data;
using System.Data.SqlClient;

namespace Triptopaz.Empty.Framework.DataBase
{
    public class DbHandler : IDisposable
    {
        #region properties
        static IDbConnection _dbConnection = null;
        #endregion properties

        #region Constructor

        public DbHandler()
        {

        }

        #endregion Constructor

        #region Method

        public static IDbConnection CreateConnection(string connectionString)
        {
            _dbConnection = new SqlConnection(connectionString);

            return _dbConnection;
        }

        /// <summary>
        /// Transaction  생성
        /// .net 의 기본 IsolationLevel 은 Serializable 이기에  기본적인 lock 이 걸릴 확률이 높음
        /// 항상 Transaction 을 아래의 메소드로 사용해야한다.
        /// </summary>
        /// <returns></returns>
        public static IDbTransaction CreateTransation()
        {
            return _dbConnection.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
        }

        #endregion Method

        #region IDisposable Support
        private bool disposedValue = false; // 중복 호출을 검색하려면

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 관리되는 상태(관리되는 개체)를 삭제합니다.
                    _dbConnection.Dispose();
                }

                // TODO: 관리되지 않는 리소스(관리되지 않는 개체)를 해제하고 아래의 종료자를 재정의합니다.
                // TODO: 큰 필드를 null로 설정합니다.

                disposedValue = true;
            }
        }

        // TODO: 위의 Dispose(bool disposing)에 관리되지 않는 리소스를 해제하는 코드가 포함되어 있는 경우에만 종료자를 재정의합니다.
        // ~DBConnection()
        // {
        //   // 이 코드를 변경하지 마세요. 위의 Dispose(bool disposing)에 정리 코드를 입력하세요.
        //   Dispose(false);
        // }

        // 삭제 가능한 패턴을 올바르게 구현하기 위해 추가된 코드입니다.
        public void Dispose()
        {
            // 이 코드를 변경하지 마세요. 위의 Dispose(bool disposing)에 정리 코드를 입력하세요.
            Dispose(true);
            // TODO: 위의 종료자가 재정의된 경우 다음 코드 줄의 주석 처리를 제거합니다.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
