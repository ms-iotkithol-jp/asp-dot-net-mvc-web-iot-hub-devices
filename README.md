# Sample of IoT Hub Device Service applicaiton based ASP.NET MVC Web App technology

Azure IoT Hub に登録されたデバイスに対し、以下を行う Web アプリケーションを、ASP.NET MVC Web App 及び、 Entity Framework テクノロジーを使って実装するサンプルを紹介する。 
- Desired Properties、Etags の更新 
- Direct Method invocation 
- C2D メッセージ送信 

Azure IoT Hub へのデバイス登録・削除や、アプリへのサインイン等の実装は含まないが、このサンプルをベースに追加が可能。

## Contents  
- [サンプルを動かす](#%E3%82%B5%E3%83%B3%E3%83%97%E3%83%AB%E3%82%92%E5%8B%95%E3%81%8B%E3%81%99)
- [サンプルの構造](#structure)
- [このサンプルをどうやって作ったか](#how-to-construct-this-sample)
- [各自の Web App 開発時の利用方法](#how-to-reuse)

-------------------------
## サンプルを動かす 
予め、Azure IoT Hub（Free で可）を作成し、いくつかデバイスを登録しておく。共有アクセスポリシーで、<b>レジストリ書き込み</b>、<b>サービス接続</b>の権限を持ったアクセスポリシーを作っておく。  
[SampleWebApp/WebAppDeviceRegistry.csproj](./SampleWebApp/WebAppDeviceRegistry.csproj) を Visual Studio 2019 で開く。  
Web.config の  
```json
  <connectionStrings>
	  <add name="AzureIoTHubConnectionString" connectionString="<- your IoT Hub connection string ->"/>
  </connectionStrings>
```
に、作成した Azure IoT Hub の、アクセスポリシーの接続文字列を設定し、デバッグ実行する。  
![web too](./images/webapp.png) 
<b>'List of devices</b> をクリックすると、Azure IoT Hub に登録されたデバイスの一覧が表示される。  
各デバイスの右端の <b>'Edit'</b> をクリックすると Desired Properties、ETags の更新用のページが表示される。 <b>'Detail'</b> をクリックすると Direct Method の Invocation と、C2D Message の送信ができるページが表示される。 

-----
## Structure 
### Models/DeviceRegistry.cs  
デバイスの登録情報と、Direct Method、C2D Message を扱うためのモデルの定義。  

### Views/DeviceRegistries  
デバイスの一覧表示、Desired Properties、ETags の更新、Direct Method、C2D Message 送信用のUIページ  

### Controllers/DeviceRegistriesController.cs  
View と Model をつなぐコントローラ。次の AzureIoTHubContext を通じて View に必要な Azure IoT Hub とのやり取りを制御する。  

### Data/AzureIoTHubContext.cs  
Azure IoT Hub からの登録データの取り出し、Desired Properties、ETags の更新、Direct Method のInvocation と戻り値の取得、C2D Message の送信を行う。  

----
## How to Construct This Sample  
基本的な流れとして、[MVC 5 を使用した Entity Framework 6 Database First](https://docs.microsoft.com/ja-jp/aspnet/mvc/overview/getting-started/database-first-development/) を参考に、データベースを用意し、Model、Controller、View を自動生成し、View、Controller を改造、Context の置き換えを行って作成。  

### Step 1 - Local DBの作成 
[Visual Studio による SQL DB 作成](https://docs.microsoft.com/ja-jp/sql/ssdt/how-to-create-a-new-database-project?view=sql-server-ver15)を参考に、<b>DeviceRegistry</b>という名前で、テーブルを作成する。スキーマは、以下の通り。

```sql
CREATE TABLE [dbo].[DeviceRegistry]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[DeviceId] NVARCHAR(50) NOT NULL,
	[DesiredProperties] TEXT NULL,
	[ReportedProperties] TEXT NULL,
	[ETags] TEXT NULL,
	[MethodName] TEXT NULL,
	[MethodPayload] TEXT NULL,
	[C2DMessage] TEXT NULL
)
```

### Step 2 - ASP.NET MVC Web application プロジェクトの作成  
Visual Studio 2019 で、 ASP.NET MVC Web application (.NET Framework) C# のテンプレートでプロジェクトを作成する。  
Nuget で、Microsoft.Azure.Devices をインストールする。

### Step 3 - Model, Controller, View の生成 
[MVC 5 を使用した Entity Framework 6 Database First](https://docs.microsoft.com/ja-jp/aspnet/mvc/overview/getting-started/database-first-development/)  に従って、DeviceRegistry テーブルを使って、Model、View Controller を作成する。  


### Step 4 - Context の追加と、Controller、View の修正  
Data フォルダーに、[Data/AzureIoTHubContext.cs](./SampleWebApp/Data/AzureIoTHubContext.cs) を追加。  
Controllers フォルダーに自動生成された <i>Xxxx</i>Controller.cs は、データベースへのアクセス用になっているので、AzureIoTHubContext にアクセスするように変更し、更に、Desired Properties、ETags の更新、Direct Method Invocation、C2D Message 送信用のアクションメソッドを追加。  
Views/DeviceRegistries の Details.chtml、Edit.cshtml に Azure IoT Hub 向けの UI 要素と、Controller の Action 起動を追加。  
Web.config に、Azure IoT Hub の接続文字列を追加。  
要らないファイルの削除。 

以上で、完成。

------
## How to Reuse  
各自の Web アプリは、以下の手順で構築すればよい。  
1. Visual Studio で、ASP.NET MVC Web App (.NET Framework) C# プロジェクトを作成
2. Nuget による Microsoft.Azure.Devices のインストール 
3. Data フォルダーを作成し、AzureIoTHubContext.cs をコピー 
4. Controller フォルダーに、DeviceRegistriesContext.cs をコピー 
5. Models フォルダーに、DeviceRegistry.cs をコピー 
6. Models フォルダーに、DeviceRegistries フォルダーを作成し、Index.cshtml, Edit.cshtml, Details.cshtml をコピー、もしくは、参考にして自作  
7. コピーしたファイルの名前空間を、作成したプロジェクトの名前空間名に変更
8. Views/Home/Index.cshtml に、DeviceRegistries/Index へのリンクを追加 
9. Web.config への Azure IoT Hub 接続文字列の追加

アプリへのサインインが必要な場合は、プロジェクト作成時に、認証機能を追加し、
![authentication setting](./images/mvcsignin.png)
[セキュアな Web アプリ開発方法](https://docs.microsoft.com/ja-jp/aspnet/core/security/authorization/secure-data?view=aspnetcore-3.1)を参考に、必要な設定を加えればよい。
