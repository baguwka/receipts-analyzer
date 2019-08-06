# receipts-analyzer
## Developer guide
### Requirements
- PostgreSQL
- .NET Core SDK 2.1
- npm

### Steps

1. Restore db from dump `\api\receipts-server\db\dump` 
1. Open pgAdmin
1. Create database
1. Click `Restore...`
1. Select file by clicking `...` at Filename field
1. Select tar file
[<img src="https://i.imgur.com/uY15710.png">](https://i.imgur.com/uY15710.png)

1. Click Restore

## Launch
Before you start, you need to fill fns_users table with FNS accounts.

[<img src="https://i.imgur.com/LbcFOFZ.png">](https://i.imgur.com/LbcFOFZ.png)

use your own username and password below (replace 'YOUR USERNAME' and 'YOUR PASSWORD' with your own)

```sql
INSERT INTO fns_users (key, username, password) VALUES('main', 'YOUR USERNAME', 'YOUR PASSWORD');
INSERT INTO fns_users (key, username, password) VALUES('test', 'YOUR USERNAME', 'YOUR PASSWORD');
```
