# Görev Yönetim Sistemi

Bu proje, **Görev Yönetim Sistemi** için bir örnek uygulama içermektedir. Bu sistem, görev yönetimi süreçlerini dijitalleştirmeyi ve kolaylaştırmayı amaçlar. Kullanıcılar, görev atamaları yapabilir, görevleri takip edebilir ve görev durumlarını güncelleyebilir.

---

## Proje Hakkında

**Görev Yönetim Sistemi** projesi, görev yönetimi süreçlerini düzenlemek için tasarlanmış bir web uygulamasıdır. Kullanıcılar, görevleri oluşturabilir, atayabilir, güncelleyebilir ve silebilir. Sistem, kullanıcıların görev durumlarını takip etmelerini ve ilgili filtreler yardımıyla görevleri daha kolay yönetmelerini sağlar.

Proje, **RESTful API** mimarisi kullanılarak geliştirilmiştir. **JWT (JSON Web Token)** tabanlı kimlik doğrulama sistemi sayesinde, kullanıcı oturumları güvenli bir şekilde yönetilmektedir. **Code First** yaklaşımı ile veritabanı modelleri oluşturulmuş ve Entity Framework Core kullanılarak yönetilmiştir.

---

## Özellikler

- **Görev Yönetimi:** Kullanıcılar görev oluşturabilir, atayabilir ve yönetebilir.
- **Filtreleme:** Kullanıcılar, görev listesi üzerinde çeşitli filtreler kullanarak belirli görevleri arayabilir.
- **Görev Durumu:** Görevlerin durumu (Beklemede, Tamamlandı, Reddedildi,Süresi Doldu) takip edilebilir.
- **Duyuru Yönetimi:** `İnsan Kaynakları Uzmanı` olan kullanıcı duyuru oluşturabilir, silebilir ve güncelleyebilir.
- **Görev Onaylama/Reddetme:** Kullanıcılar kendilerine atanan görevleri onaylayabilir veya reddedebilir.
- **Kullanıcı Profili:** Kullanıcılar profil bilgilerini görüntüleyebilir ve güncelleyebilir.
- **Oturum Yönetimi:**
  - **Beni Hatırla:** Giriş ekranında bulunan "Beni Hatırla" butonu ile kullanıcıların oturumları 1 gün boyunca açık kalabilir. Aksi takirde kullanıcılar herhangi bir işlem yapadıklarında oturumları 15 dakkikanın ardından sonlanmaktadır.
- **Yetkilendirme ve Yönlendirme:**
  - **İnsan Kaynakları Uzmanı** olan kişiler departman ekleyebilir, kullanıcı ekleyebilir ve kullanıcı silebilir.
  - Bu yetkiye sahip olmayan kişiler, departman ve kullanıcı yönetimi sayfalarına girmeye çalıştıklarında özel bir "access denied" sayfasına yönlendirilir.
  - Mevcut olmayan sayfalara erişim durumunda, özel bir "not found" sayfasına yönlendirilirler.
  - İlgili insan kaynakları bilgisi, kullanıcı token'ından departman bilgisi kontrol edilerek doğrulanır.
- **RESTful API Kullanımı:** Sistem, RESTful API yapısını kullanarak modüler ve ölçeklenebilir bir yapı sunar.
- **JWT Tabanlı Kimlik Doğrulama:** JSON Web Token (JWT) kullanılarak güvenli kimlik doğrulama sağlanır.
- **Code First Yaklaşımı:** Entity Framework Core ile veritabanı yapısı, kod bazlı olarak (Code First) oluşturulmuş ve yönetilmiştir.
- **Bootstrap ve JavaScript Kullanımı:** Kullanıcı arayüzünde Bootstrap kullanılmış olup, bazı interaktif özellikler için JavaScript uygulanmıştır.
- **FontAwesome:** Projede ikon setleri için FontAwesome kütüphanesi kullanılmıştır.
- **Çok Katmanlı Mimari:** Proje, sürdürülebilirlik ve genişletilebilirlik için çok katmanlı bir mimari üzerine inşa edilmiştir.

---

## Gereksinimler

- .NET 6.0
- ASP.NET Core MVC
- Entity Framework Core
- SQL Server
- Visual Studio veya Visual Studio Code
- Bootstrap
- JavaScript
- FontAwesome
- AutoMapper
- Sweetalert
---

## Utility Katmanı

**Utility katmanı**, projede genel amaçlı yardımcı sınıfların ve yapıların tanımlandığı katmandır. Bu katman, uygulamanın farklı yerlerinde kullanılabilecek ortak fonksiyonları, sabit değerleri ve genişletme metodlarını içerir. İşte bu katmandaki bazı önemli sınıf ve yapılar:

### `SD` Sınıfı

`SD` (Static Details) sınıfı, projede kullanılan sabit değerlerin ve yapıların saklandığı bir sınıftır. Bu sınıf, projede merkezi bir sabit veriler kaynağı sağlar. İçeriğinde, API çağrıları için kullanılan HTTP yöntemlerini (GET, POST, PUT, DELETE) temsil eden bir enum (`ApiType`) ve kullanıcı oturumları için kullanılan token değerinin saklandığı bir `SessionToken` sabiti bulunmaktadır.

- **ApiType Enum:** Bu enum, HTTP istek türlerini temsil eder ve API çağrıları sırasında kullanılabilir.
  - **GET:** Veri almak için kullanılır.
  - **POST:** Yeni veri oluşturmak için kullanılır.
  - **PUT:** Mevcut veriyi güncellemek için kullanılır.
  - **DELETE:** Veriyi silmek için kullanılır.

- **SessionToken:** Kullanıcı oturumları için kullanılan JWT (JSON Web Token) token'ını saklamak için kullanılan bir sabittir.

```csharp
 public class SD
 {
     public enum ApiType
     {
         GET,
         POST,
         PUT,
         DELETE
     }
 }
```

### `TaskStatusEnum` Enum

`TaskStatusEnum`, görevlerin durumlarını temsil eden bir numaralandırmadır (enum). Bu enum, görevlerin hangi durumda olduğunu belirlemek için kullanılır ve aşağıdaki durumları içerebilir:
- **Beklemede**: Görev beklemede.
- **Tamamlandı**: Görev tamamlandı.
- **Reddedildi**: Görev reddedildi.
- **SüreDoldu**: Görevin süresi doldu.

Bu enum, görev durumlarını standartlaştırmak ve bu durumların yönetimini kolaylaştırmak amacıyla kullanılır.


### `UserExtension` Sınıfı

`UserExtension` sınıfı, `ClaimsPrincipal` nesnesine eklenen genişletme metodlarını içerir. Bu genişletme metodları, mevcut `User` nesnesine ekstra işlevler ekler. Örneğin, kullanıcının "İnsan Kaynakları Uzmanı" departmanına ait olup olmadığını kontrol etmek için kullanılır.

- **IK Genişletme Metodu:** Bu metod, kullanıcının (`ClaimsPrincipal`) departman bilgisini kontrol eder ve kullanıcının "İnsan Kaynakları Uzmanı" olup olmadığını belirler. Eğer kullanıcı "İnsan Kaynakları Uzmanı" ise `true`, aksi halde `false` döner.

```csharp
 public static class UserExtension
 {
     public static bool IK(this ClaimsPrincipal user)
     {
         return user.Claims.FirstOrDefault(d => d.Type == "DepartmentName")?.Value == "İnsan Kaynakları Uzmanı";
     }
 }
```
- **Cinsiyet Genişletme Metodu:** Bu metod, kullanıcının (`ClaimsPrincipal`) cinsiyet bilgisini kontrol eder ve kullanıcının cinsiyetinin "Female" olup olmadığını belirler. Eğer kullanıcının cinsiyet "Female" ise `true`, aksi halde `false` döner.

```csharp
 public static bool IsFemale(this ClaimsPrincipal user)
 {
     return user.Claims.FirstOrDefault(c => c.Type == "Gender")?.Value == "Female";
 }
```

---

## Business Katmanı

**Business katmanı**, uygulamanın iş mantığını ve veri işleme süreçlerini yöneten katmandır. Bu katman, veri tabanı ile uygulama arasındaki etkileşimi yönetir ve iş kurallarını uygular. İşte bu katmandaki bazı önemli sınıf ve arayüzlerin açıklamaları:

### Repository Arayüzü (`IRepository<T>`)

`IRepository<T>` genel bir repository arayüzüdür ve belirli bir veri modeli (Entity) üzerinde gerçekleştirilmesi gereken temel CRUD (Create, Read, Update, Delete) işlemlerini tanımlar. Bu arayüz, generic yapıda olduğundan, farklı veri modelleriyle çalışabilme esnekliği sağlar.

- **GetAll**: Tüm verileri veya belirli bir koşula uyan verileri getirir.
- **Get**: Belirli bir veri kaydını getirir.
- **Create**: Yeni bir veri kaydı oluşturur.
- **Delete**: Mevcut bir veri kaydını siler.
- **Save**: Veritabanına yapılan değişiklikleri kaydeder.

### Kullanıcı Yönetimi Arayüzü (`IUserRepository`)

`IUserRepository`, `User` entity'si ile ilgili iş mantığını uygulayan arayüzdür. Bu arayüz, `IRepository<User>` arayüzünden türetilmiş olup, kullanıcılarla ilgili ek işlevler içerir.

- **UpdateUser**: Kullanıcı bilgilerini günceller.
- **GetUserWithDetails**: Kullanıcıyı detaylı bilgileriyle birlikte getirir.
- **GetAllUserWithDetails**: Tüm kullanıcıları detaylı bilgileriyle birlikte getirir.
- **Login**: Kullanıcı giriş işlemini gerçekleştirir.
- **GetUserTask**: Belirli bir kullanıcıya atanmış görevleri getirir.

### Görev Yönetimi Arayüzü (`IToDoTaskRepository`)

`IToDoTaskRepository`, `ToDoTask` entity'si ile ilgili iş mantığını uygulayan arayüzdür. Bu arayüz, `IRepository<ToDoTask>` arayüzünden türetilmiş olup, görevlerle ilgili ek işlevler içerir.

- **UpdateTask**: Görev bilgilerini günceller.
- **GetTaskById**: Görev kimliğine göre görev getirir.

### Departman Yönetimi Arayüzü (`IDepartmentRepository`)

`IDepartmentRepository`, `Department` entity'si ile ilgili iş mantığını uygulayan arayüzdür. Bu arayüz, `IRepository<Department>` arayüzünden türetilmiş olup, departmanlarla ilgili ek işlevler içerir.

- **UpdatDepartment**: Departman bilgilerini günceller.

### Duyuru Yönetimi Arayüzü (`IAnnouncementRepository`)

`IAnnouncementRepository`, `Announcement` entity'si ile ilgili iş mantığını uygulayan arayüzdür. Bu arayüz, `IRepository<Announcement>` arayüzünden türetilmiş olup, duyurularla ilgili ek işlevler içerir.

- **UpdateAnnouncement**: Duyuru bilgilerini günceller.

### API Yanıt Sınıfı (`APIResponse`)

`APIResponse`, API'lerden dönen yanıtları yapılandırmak için kullanılan bir sınıftır. Bu sınıf, bir API çağrısının sonucunu ve durumunu standart bir formatta sunar.

- **StatusCode**: API çağrısının HTTP durum kodunu tutar (örneğin, 200 OK, 404 Not Found).
- **IsSuccess**: API çağrısının başarılı olup olmadığını belirtir.
- **Errors**: Eğer çağrı başarısız olduysa, hata mesajlarının listesini içerir.
- **Result**: API çağrısının sonucunda dönen veriyi içerir.

Bu yapı sayesinde, API yanıtları düzenli, izlenebilir ve güvenli bir şekilde yönetilir. İstemci tarafında ise bu yanıtlar kolayca işlenebilir.

---

## Data Katmanı

**Data** katmanı, projenin temel veri modellerini ve bu modellerin birbirleriyle olan ilişkilerini içerir. Bu katmanda yer alan Entity sınıfları, veritabanı tablolarını temsil eder ve Entity Framework Core tarafından veritabanı işlemleri için kullanılır.

### TaskManagerContext Sınıfı

TaskManagerContext sınıfı, Entity Framework Core kullanılarak veritabanı işlemlerinin yönetildiği temel sınıftır. Bu sınıf, veritabanı bağlantısı ve veritabanı üzerinde gerçekleştirilecek işlemler için gerekli olan yapılandırmaları içerir.

```csharp
public class TaskManagerContext : DbContext
{
    public TaskManagerContext(DbContextOptions<TaskManagerContext> options) : base(options)
    {
    }

    // Veritabanında yer alacak tablolar
    public DbSet<User> Users { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<ToDoTask> Task { get; set; }
    public DbSet<Announcement> Announcements { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // ToDoTask Entity'sinin ilişkileri
        modelBuilder.Entity<ToDoTask>()
            .HasOne(t => t.CreaterUser)  
            .WithMany(u => u.CreatedTasks)
            .HasForeignKey(u => u.CreaterUserId)
            .OnDelete(DeleteBehavior.Restrict);  

        modelBuilder.Entity<ToDoTask>()
            .HasOne(t => t.AsaignedUser)  
            .WithMany(u => u.Tasks)
            .HasForeignKey(u => u.AsaignedUserId)
            .OnDelete(DeleteBehavior.Restrict);  

        modelBuilder.Entity<ToDoTask>()
            .HasOne(t => t.Department)    
            .WithMany(d => d.Tasks)
            .HasForeignKey(t => t.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict); 

        // User Entity'sinin ilişkisi
        modelBuilder.Entity<User>()
            .HasOne(u => u.Department)    
            .WithMany(d => d.Users)
            .HasForeignKey(u => u.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);  // Silme işlemi kısıtlı
    }
}
```

`TaskManagerContext` sınıfı ile tanımlanan veritabanı tablolarının Entity sınıfları ve aralarındaki ilişkiler yukarıda belirtilmiştir. Bu sınıf, Entity Framework Core kullanılarak oluşturulmuş olup, veritabanı işlemlerinin ve tablolar arası ilişkilerin yönetimini sağlar. 

Bu yapıda, kullanıcılar ve görevler arasındaki ilişkiler ile departmanlar ve görevler arasındaki ilişkiler doğru ve güvenli bir şekilde yönetilmiştir. Özellikle `DeleteBehavior.Restrict` kullanılarak, bağlı olan verilerin kazara silinmesini engelleyecek şekilde yapılandırma yapılmıştır.

---
    
### Entity Sınıfları ve İlişkileri

#### 1. **User (Kullanıcı) Sınıfı**

- **Properties:**
  - `Id`: Kullanıcının benzersiz kimlik numarası.
  - `Name`: Kullanıcının adı.
  - `Gender`: Kullanıcının cinsiyeti.
  - `LastName`: Kullanıcının soyadı.
  - `Email`: Kullanıcının e-posta adresi.
  - `Education`: Kullanıcının eğitim durumu.
  - `Address`: Kullanıcının adres bilgisi.
  - `Country`: Kullanıcının ülke bilgisi.
  - `State`: Kullanıcının şehir bilgisi.
  - `PhoneNumber`: Kullanıcının telefon bilgisi.
  - `Department`: Kullanıcının bağlı olduğu departman. Bu, `Department` entity'si ile olan bir ilişkidir.
  - `DepartmentId`: Kullanıcının hangi departmana ait olduğunu belirten yabancı anahtar (foreign key).
  - `Tasks`: Kullanıcıya atanmış görevlerin listesi. Bu, `ToDoTask` entity'si ile olan bir ilişkidir.
  - `CreatedTasks`: Kullanıcının oluşturduğu görevlerin listesi. Bu da `ToDoTask` entity'si ile olan bir ilişkidir.

**İlişkiler:**
- `User` sınıfı ile `Department` sınıfı arasında bire çok (one-to-many) bir ilişki vardır. Her kullanıcı bir departmana aittir ve bir departmanın birden fazla kullanıcısı olabilir.
- `User` sınıfı ile `ToDoTask` sınıfı arasında bire çok (one-to-many) bir ilişki vardır. Her kullanıcıya birden fazla görev atanabilir ve bir kullanıcı birden fazla görev oluşturabilir.

#### 2. **Department (Departman) Sınıfı**

- **Properties:**
  - `Id`: Departmanın benzersiz kimlik numarası.
  - `DepartmentName`: Departmanın adı.
  - `Users`: Bu departmana bağlı olan kullanıcıların listesi. `User` entity'si ile olan ilişkidir.
  - `Tasks`: Bu departmanda yer alan görevlerin listesi. `ToDoTask` entity'si ile olan ilişkidir.

**İlişkiler:**
- `Department` sınıfı ile `User` sınıfı arasında bire çok (one-to-many) bir ilişki vardır. Bir departmanın birden fazla kullanıcısı olabilir, ancak her kullanıcı yalnızca bir departmana aittir.
- `Department` sınıfı ile `ToDoTask` sınıfı arasında da bire çok (one-to-many) bir ilişki vardır. Bir departman, birden fazla görev içerebilir.

#### 3. **ToDoTask (Görev) Sınıfı**

- **Properties:**
  - `Id`: Görevin benzersiz kimlik numarası.
  - `TaskName`: Görevin adı.
  - `Department`: Görevin ait olduğu departman. Bu, `Department` entity'si ile olan bir ilişkidir.
  - `DepartmentId`: Görevin hangi departmana ait olduğunu belirten yabancı anahtar (foreign key).
  - `CreaterUser`: Görevi oluşturan kullanıcı. Bu, `User` entity'si ile olan bir ilişkidir.
  - `CreaterUserId`: Görevi oluşturan kullanıcının kimlik numarası.
  - `AsaignedUser`: Göreve atanan kullanıcı. Bu, `User` entity'si ile olan bir ilişkidir.
  - `AsaignedUserId`: Göreve atanan kullanıcının kimlik numarası.
  - `Status`: Görevin durumu. Bu, `TaskStatusEnum` ile tanımlanmış bir durum bilgisidir.
  - `AssignmentDate`: Görevin kullanıcıya atandığı tarih bilgisidir.

#### 4. **Announcement (Duyuru) Sınıfı**

- **Properties:**
  - `Id`: Duyurunun benzersiz kimlik numarası.
  - `Title`: Duyurunun başlığı.
  - `Content`: Duyurunun içeriği.
  - `CreatedDate`: Duyurunun oluşturulduğu tarih.
  - `UpdatedDate`: Duyurunun güncelleştirildiği tarih.
  - `AuthorId`: Duyuruyu oluşturan kullanıcının kimlik numarası.
  - `AuthorName`: Duyuruyu oluşturan kullanıcı.

**İlişkiler:**
- `ToDoTask` sınıfı ile `Department` sınıfı arasında bire çok (one-to-many) bir ilişki vardır. Her görev, belirli bir departmana aittir.
- `ToDoTask` sınıfı ile `User` sınıfı arasında iki bire çok (one-to-many) ilişki vardır:
  - `CreaterUser`: Her görev bir kullanıcı tarafından oluşturulur.
  - `AsaignedUser`: Her görev bir kullanıcıya atanabilir.
- `ToDoTask` sınıfı ile `TaskStatusEnum` arasında bir enum ilişkisi vardır. Bu enum, görevlerin durumunu belirtir.

---

### Entity Sınıflarının Genel İlişkileri

- **Department** ve **User** sınıfları arasında bir departmanın birden fazla kullanıcıya sahip olabileceği, ancak her kullanıcının yalnızca bir departmana ait olabileceği bire çok (one-to-many) ilişkisi bulunmaktadır.
- **User** ve **ToDoTask** sınıfları arasında bir kullanıcının birden fazla görev oluşturabileceği ve birden fazla göreve atanabileceği iki ayrı bire çok (one-to-many) ilişkisi bulunmaktadır.
- **Department** ve **ToDoTask** sınıfları arasında bir departmanın birden fazla göreve sahip olabileceği bire çok (one-to-many) ilişkisi vardır.

Bu yapı, **Görev Yönetim Sistemi**'nde kullanıcıların ve departmanların görevleri nasıl oluşturup yönetebileceğini belirleyen temel veri modelini oluşturur. Bu sayede, sistemdeki görevler ve bu görevlerin hangi kullanıcılar tarafından oluşturulup, hangi kullanıcılara atandığı veritabanında doğru ve verimli bir şekilde saklanabilir.

---

## DTO (Data Transfer Object) Katmanı

**DTO (Data Transfer Object)**, veritabanından gelen veya veritabanına gönderilen veri modellerini temsil eden sınıflardır. DTO'lar, veri taşımak için kullanılan basit nesnelerdir ve uygulama katmanları arasında veri transferi için kullanılır. Bu nesneler, genellikle yalnızca veri tutar ve iş mantığını içermez. DTO'lar, özellikle büyük ve karmaşık projelerde, veriyi güvenli ve kontrollü bir şekilde taşımak için tercih edilir. Veriyi taşırken gereksiz bilgilerin iletilmemesi ve veri manipülasyonunun önlenmesi amacıyla DTO'lar kullanılmaktadır.

Bu projede de **Görev Yönetim Sistemi** kapsamında çeşitli DTO'lar kullanılmıştır. Aşağıda, projede kullanılan DTO sınıflarının listesini ve hangi işlevleri yerine getirdiklerini bulabilirsiniz:

### Kullanılan DTO'lar:

- **DepartmentDtos**
  - `DepartmentCreateDto.cs`: Yeni bir departman oluşturmak için kullanılan DTO sınıfıdır.
  - `DepartmentDto.cs`: Departman bilgilerini taşıyan temel DTO sınıfıdır.
  - `DepartmentUpdateDto.cs`: Mevcut bir departmanın güncellenmesi için kullanılan DTO sınıfıdır.

- **TaskDtos**
  - `TaskCreateDto.cs`: Yeni bir görev oluşturmak için kullanılan DTO sınıfıdır.
  - `TaskDto.cs`: Görev bilgilerini taşıyan temel DTO sınıfıdır.
  - `TaskUpdateDto.cs`: Mevcut bir görevin güncellenmesi için kullanılan DTO sınıfıdır.

- **UserDtos**
  - `LoginRequestDto.cs`: Kullanıcı giriş isteğini taşıyan DTO sınıfıdır.
  - `LoginResponseDto.cs`: Kullanıcı girişine yanıt olarak dönen DTO sınıfıdır.
  - `UserCreateDto.cs`: Yeni bir kullanıcı oluşturmak için kullanılan DTO sınıfıdır.
  - `UserDto.cs`: Kullanıcı bilgilerini taşıyan temel DTO sınıfıdır.
  - `UserUpdateDto.cs`: Mevcut bir kullanıcının güncellenmesi için kullanılan DTO sınıfıdır.

 - **AnnouncementDtos**
  - `AnnouncementCreateDto.cs`: Yeni bir duyuru oluşturmak için kullanılan DTO sınıfıdır.
  - `AnnouncementDto.cs`: Duyuru bilgilerini taşıyan temel DTO sınıfıdır.
  - `AnnouncementUpdateDto.cs`: Mevcut bir duyurunun güncellenmesi için kullanılan DTO sınıfıdır.

Bu DTO'lar sayesinde, proje içerisindeki veriler modüler bir şekilde işlenir ve katmanlar arasında güvenli bir veri iletişimi sağlanır. Her bir DTO sınıfı, ilgili veri modelini taşımakla sorumludur ve proje içerisindeki API çağrılarında kullanılmaktadır. Bu yapı, veri transfer süreçlerini optimize eder ve projenin sürdürülebilirliğini artırır.

---

## API Katmanı

**API katmanı**, uygulamanın dış dünyaya açılan kapısıdır ve bu katman, istemcilerden (frontend veya mobil uygulamalardan) gelen HTTP isteklerini işler. API katmanında yer alan **Controller** sınıfları, belirli veri işlemleri için gerekli olan iş mantığını sağlar ve bu işlemleri **Business** katmanındaki ilgili repository sınıfları üzerinden gerçekleştirir.

### Controllerlar ve İstek Yönetimi

Bu katmanda yer alan her **Controller** sınıfı, belirli bir veri modeliyle (örneğin, `User`, `Task`, `Department`, `Announcement`) ilgili API isteklerini yönetir. Controller sınıfları, istemciden gelen isteği alır, gerekli doğrulamaları yapar, ilgili repository üzerinden veri işlemlerini gerçekleştirir ve sonuçları bir **APIResponse** nesnesi olarak döndürür.

#### Örnek: TaskController

Örneğin, `TaskController`, görevlerle ilgili API isteklerini yönetir. Bu sınıf, görev oluşturma, güncelleme, silme gibi işlemler için **IToDoTaskRepository** arayüzünü kullanır. Dependency Injection (DI) kullanılarak `TaskController` içerisine gerekli bağımlılıklar enjekte edilir:

```csharp
private readonly IToDoTaskRepository _taskRepository;
private readonly IMapper _mapper;
private readonly APIResponse _apiResponse;

public TaskController(IToDoTaskRepository taskRepository, IMapper mapper)
{
    _taskRepository = taskRepository;
    _mapper = mapper;
    this._apiResponse = new APIResponse();
}
```
## İsteklerin İşlenmesi ve Yanıt Dönüşü
İstemciden gelen bir istek, ilgili `Controller` sınıfında işlenir ve işlemin sonucuna göre bir yanıt oluşturulur. Başarılı bir işlem sonrasında, `APIResponse` nesnesi üzerinden işlem sonucu, durum kodu ve işlenen veri döndürülür:

```csharp
_apiResponse.IsSuccess = true;
_apiResponse.StatusCode = System.Net.HttpStatusCode.Created;
_apiResponse.Result = _mapper.Map<TaskDto>(task);
return _apiResponse;
```
Eğer bir hata oluşursa, hata mesajı ve durum kodu yine `APIResponse` üzerinden döndürülür:

```csharp
catch (Exception ex)
{
    _apiResponse.IsSuccess = false;
    _apiResponse.Errors = new List<string> { ex.ToString() };
    _apiResponse.StatusCode = System.Net.HttpStatusCode.InternalServerError;
}
return _apiResponse;
```

## Dependency Injection ve Program.cs
API katmanında Dependency Injection (DI) kullanılarak, `Repository` sınıfları `Controller`'lara enjekte edilir. Bu işlem, `Program.cs` dosyasında gerçekleştirilir. `AddScoped` metodu ile repository sınıfları örneklenir ve gerekli bağımlılıklar yapılandırılır:

```csharp
builder.Services.AddScoped<IToDoTaskRepository, ToDoTaskRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IAnnouncementRepository, AnnouncementRepository>();
```


Ayrıca, veritabanı bağlantısı ve kimlik doğrulama (authentication) ayarları da `Program.cs` içerisinde yapılır:

```csharp
builder.Services.AddDbContext<TaskManagerContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var key = builder.Configuration.GetValue<string>("ApiSettings:Secret");

builder.Services.AddAuthentication(a =>
{
    a.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    a.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
        ValidateIssuer = false,
        ValidateAudience = false,
    };
});
```
Bu yapı, API katmanındaki controller'ların, iş mantığı ve veri erişim katmanlarıyla düzgün bir şekilde çalışmasını sağlar ve bu katmanlar arasındaki bağımlılıkları yönetir.

---

## Web Katmanı

**Web katmanı**, bu proje ASP.NET Core MVC mimarisi kullanılarak geliştirilmiştir. Bu katman, uygulamanın kullanıcı arayüzünü oluşturur ve kullanıcıların çeşitli işlemleri gerçekleştirebileceği sayfaları yönetir.

### wwwroot Klasörü

`wwwroot` klasörü, projenin statik dosyalarının (CSS, JavaScript, resimler vb.) saklandığı yerdir. Bu projede, **Bootstrap** kütüphanesi kuruludur ve stil dosyaları burada bulunur. Ayrıca, `images` klasöründe projede kullanılan resimler depolanmaktadır.

### AutoMapper

`AutoMapper`, veri transfer nesneleri (DTO) ve ViewModel'ler arasında nesne eşlemesi yapmak için kullanılır. `MapperConfig.cs` dosyasında, projede kullanılan tüm mapping konfigürasyonları tanımlanmıştır.

### Controllers

`Controllers` klasörü, kullanıcılardan gelen HTTP isteklerini işleyen ve ilgili View'leri döndüren sınıfları içerir. Bu projede, aşağıdaki kontrolörler bulunmaktadır:

- **AuthController:** Kullanıcı kimlik doğrulama işlemlerini yönetir. Kullanıcıların giriş yapması gibi işlemleri içerir.
- **DepartmentController:** Departmanlarla ilgili işlemleri yönetir. Yeni departman oluşturma, güncelleme gibi işlemleri içerir.
- **HomeController:** Genel sayfaların yönetimini sağlar. Örneğin, anasayfa, erişim engellendi sayfası ve gizlilik politikası gibi genel sayfaları döndürür.
- **TaskController:** Görevlerle ilgili işlemleri yönetir. Görev oluşturma, güncelleme, silme ve detaylarını görüntüleme gibi işlemleri içerir.
- **UserController:** Kullanıcılarla ilgili işlemleri yönetir. Kullanıcı profili, kullanıcı oluşturma, güncelleme gibi işlemleri içerir.

Bu kontrolörler, `Services` klasörü altında tanımlanan servisler aracılığıyla backend ile iletişim kurar. Gelen veriyi DTO ve ViewModel'lere mapleyerek frontend'e sunar.

### Models

`Models` klasörü, projede kullanılan veri transfer nesneleri (DTO) ve ViewModel'leri içerir. Bu dosya yapısı, verilerin frontend ile backend arasında taşınmasını ve işlenmesini sağlar. Burada tanımlı bazı önemli sınıflar şunlardır:

- **APIRequest:** API'ye gönderilecek taleplerin yapılandırılmasında kullanılır. Bu sınıf, istek türünü (`ApiType`), hedef URL'yi (`Url`), ve gönderilecek veriyi (`Data`) içerir.

```csharp
 public class APIRequest
 {
     public ApiType ApiType { get; set; } = ApiType.GET;
     public string Url { get; set; }
     public object Data { get; set; }
 }
```

- **APIResponse:** API'den dönen yanıtların yapılandırılmasında kullanılır. Bu sınıf, yanıtın başarı durumunu (`IsSuccess`), HTTP durum kodunu (`StatusCode`), hata mesajlarını (`Errors`), ve dönen veriyi (`Result`) içerir.

```csharp
 public class APIResponse
{
    public HttpStatusCode StatusCode { get; set; }
    public bool IsSuccess { get; set; }
    public List<string> ErrorMessages { get; set; }
    public object result { get; set; }
}
```

- **ViewModel'ler (VM):** Kullanıcı arayüzünde kullanılan veri modellerini tanımlar. Örneğin, `TaskCreateVM`, yeni bir görev oluşturma ekranında kullanılan veri modelidir. Bu modeller, kullanıcı arayüzü ile veri işleme arasında bir köprü görevi görür. ViewModel'ler, kullanıcı arayüzünden gelen verileri toplar, doğrular ve kontrolörlere aktarır.

### Services

`Services` klasörü, uygulamanın iş mantığını yöneten ve API ile olan tüm etkileşimleri gerçekleştiren sınıfları içerir. Her servis, ilgili API işlemlerini gerçekleştirir ve gelen veriyi kontrolörlere iletir. Her servis sınıfının bir arayüzü (`IService`) vardır, bu da Dependency Injection (DI) yoluyla bu servislerin kullanılmasını sağlar.

- **Örnek:** `TaskService` sınıfı, görev oluşturma, güncelleme ve silme işlemleri gibi görev yönetimi ile ilgili tüm API çağrılarını yönetir. Bu sınıf, `BaseService`'ten türetilir ve `ITaskService` arayüzünü uygular. Servis, `IHttpClientFactory` aracılığıyla API'ye istek gönderir ve API'den gelen yanıtları işler.

```csharp
public class TaskService : BaseService, ITaskService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private string taskUrl;

    public TaskService(IHttpClientFactory httpClientFactory, IConfiguration configuration) : base(httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
        taskUrl = configuration.GetValue<string>("ServiceUrls:TaskManagementAPI");
    }

    public Task<T> CreateTask<T>(TaskCreateDto dto)
    {
        return Send<T>(new APIRequest()
        {
            ApiType = SD.ApiType.POST,
            Data = dto,
            Url = taskUrl + "/api/Task/CreateTask/"
        });
    }
}
```
Bu örnekte, `taskUrl` değeri `appsettings.json` dosyasındaki `"ServiceUrls:TaskManagementAPI"` ayarından alınmaktadır. Bu ayar, API'nin taban URL'sini belirler ve HTTP isteklerinin doğru sunucuya yönlendirilmesini sağlar.

`appsettings.json` dosyasındaki yapılandırma şu şekildedir:

```json
{
  "ServiceUrls": {
    "TaskManagementAPI": "https://localhost:7178"
  }
}
```
Bu yapılandırma, `TaskManagementAPI` servisini `https://localhost:7178 adresine yönlendirir`, bu da `TaskService` sınıfında yapılan tüm API çağrılarının bu adrese yapılmasını sağlar.

### Views

`Views` klasörü, kullanıcı arayüzünü oluşturmak için kullanılan Razor sayfalarını içerir. Bu klasörde, her bir kontrolör için ayrı bir alt klasör bulunur ve her klasör, ilgili kontrolörün döndürdüğü sayfaları barındırır.

- **Auth:** Kullanıcı kimlik doğrulama ile ilgili sayfaları içerir.
  - `Login.cshtml`: Kullanıcıların sisteme giriş yapmalarını sağlayan sayfa.

- **Department:** Departmanlarla ilgili sayfaları içerir.
  - `DepartmentCreate.cshtml`: Yeni bir departman oluşturma sayfası.

- **Home:** Genel sayfaları içerir.
  - `Home`: Tüm duyruuların ve bu duyurulara ait işlemlerin yapılabilmesi için gerekli olan sayfalara yönlendirmelerin 
     olduğu sayfa.
  - `Create`: Duyuru oluşturabilmek için görüntülenen sayfa.
  - `Update`: Duyuru güncelleştirebilmek için görüntülenen sayfa.
  - `AccessDenied.cshtml`: Erişim izni olmayan kullanıcılar için görüntülenen sayfa.
  - `NotFoundPage.cshtml`: Bulunamayan sayfalar için gösterilen hata sayfası.
  - `Privacy.cshtml`: Gizlilik politikası sayfası.

- **Task:** Görevlerle ilgili sayfaları içerir.
  - `Create.cshtml`: Yeni bir görev oluşturma sayfası.
  - `TaskDetails.cshtml`: Bir görevin detaylarını görüntüleme sayfası.
  - `Update.cshtml`: Mevcut bir görevi güncelleme sayfası.

- **User:** Kullanıcılarla ilgili sayfaları içerir.
  - `GetAllUsers.cshtml`: Tüm kullanıcıları listeleme sayfası.
  - `Profile.cshtml`: Kullanıcının profil bilgilerini görüntüleme sayfası.
  - `UpdateProfile`: Kullanıcıların profil bilgilerini güncelleyebildikleri ekran.
  - `UserCreate.cshtml`: Yeni bir kullanıcı oluşturma sayfası.
  - `UserList.cshtml`: Kullanıcıları listeleme sayfası.
  - `UsersTask.cshtml`: Kullanıcının kendisine atanmış görevleri görüntüleme sayfası.

- **Shared:** Uygulamanın tüm sayfaları için ortak olan Razor bileşenlerini içerir.
  - `_Layout.cshtml`: Uygulamanın genel yapısını tanımlayan düzen sayfası.

`Views` klasörü, MVC mimarisinde "View" katmanını temsil eder ve bu sayede kullanıcıya görsel bir arayüz sunar.


---

## Kurulum

1. Projeyi klonlayın:
```sh
    git clone https://github.com/Cengizhanyildiz14/TaskManagementSystem.git
```

2. Proje dizinine gidin:
```sh
    cd TaskManagementSystem
 ```
   
3. Gerekli bağımlılıkları yükleyin:
```sh
    dotnet restore
```
    
4. **Appsettings.json Konfigürasyonu:**

   Projeyi kullanmadan önce `appsettings.json` dosyasındaki bağlantı ayarlarını ve gizli anahtarları kendi sisteminize göre düzenlemeniz gerekmektedir. Örneğin:
```json
    "ConnectionStrings": {
      "default": "server=YOUR_SERVER_NAME; Database=YOUR_DATABASE_NAME; integrated security=true; encrypt=false"
    },
    "ApiSettings": {
      "Secret": "YOUR_SECRET_KEY"
    }
``` 
- **ConnectionStrings:** `server`, `Database`, ve diğer bağlantı ayarlarını kendi sisteminize göre düzenleyin.
   - **ApiSettings:** `Secret` anahtarını, güvenliğinizi sağlamak için kendi özel ve gizli bir anahtarla değiştirin.

5. Veritabanını oluşturun:
```sh
    dotnet ef database update
```

6. Uygulamayı çalıştırın:
```sh
    dotnet run
```

---
    
## Kullanım

- **Giriş Yapma:** Kullanıcılar e-posta adresleri ile sisteme giriş yapabilir.
- **Dyurular:** Kullanıcılar tüm duyuruları görebilir ayrıca İnsan Kaynakları Uzmanı olan kişiler bu duyuruları güncelleyebilir, silebilir veya yeni duyuru oluşturabilir.
- **Beni Hatırla:** Giriş sırasında "Beni Hatırla" butonunu seçerek, oturumunuzu 1 gün boyunca açık tutabilirsiniz.
- **Görev Oluşturma:** Ana sayfadan yeni bir görev oluşturabilir ve ilgili kullanıcılara atayabilirsiniz.
- **Görev Güncelleme ve Silme:** Kendi oluşturduğunuz görevleri güncelleyebilir veya silebilirsiniz.
- **Görev Onaylama/Reddetme:** Atanan görevleri onaylayabilir veya reddedebilirsiniz.
- **Profil Yönetimi:** Kullanıcılar profil bilgilerini görüntüleyebilir ve aynı zamanda profil bilgilerini güncelleyebilir.
- **Yetkilendirme:** İnsan Kaynakları Uzmanı olan kişiler departman ve kullanıcı yönetimi işlemleri yapabilir.
- **Yönlendirme:** Yetkiniz olmayan sayfalara erişmeye çalıştığınızda veya mevcut olmayan bir sayfaya yönlendirildiğinizde, sistem sizi ilgili özel sayfalara yönlendirir.

---

## Ekran Görüntüleri

- **Giriş Sayfası:** Kullanıcıların sisteme giriş yaptığı ekran.

  <img src="https://github.com/user-attachments/assets/b72cd4bd-f641-4418-bca9-a0c43e456fea" alt="Giriş Sayfası" width="600"/>
---
- **Anasayfa:** Kullanıcıların duyuruları takip edebildiği ekran..

  <img src="https://github.com/user-attachments/assets/59ceca9e-8e4d-44ba-a6da-52c0b3a9bf75" alt="Anasayfa" width="600"/>
---
- **Anasayfa(İK Uzmanı için):** `İnsan Kaynakları Uzmanı` olan kullanıcılar için duyurular ekranı. Diğer kullanıcılara ek olarak burada duyuruları oluşturabilme, düzenleyebilme ve silebilme yetkileri mevcuttur.

  <img src="https://github.com/user-attachments/assets/032d98cf-7586-4656-a7d9-0bcaa59c94bb" alt="Anasayfa" width="600"/>
---
- **Duyuru Oluşturma Ekranı(İK Uzmanı için):** Yeni duyuru ekleme ekranı. (Bu kısımdan önce sayfaların boyutlarından ötürü gözükmeyen ama footer'da yer alan Google haritalardan konum bilgisini görebilmektesiniz.)

  <img src="https://github.com/user-attachments/assets/2557db93-67ed-446e-994d-e55f56f32582" alt="Görevlerim Sayfası" width="600"/>
---
- **Duyuru Güncelleme Ekranı(İK Uzmanı için):** Duyuru güncelleme ekranı.

  <img src="https://github.com/user-attachments/assets/306ed4d4-886a-40a6-883e-6caa7d44aca3" alt="Profil Bilgilerim Sayfası"/>
---
- **Duyuru Silme Uyarısı(İK Uzmanı için):** Duyuruyu silmek istediğimde karşımıza çıkan uyarı pop-up penceresi.

  <img src="https://github.com/user-attachments/assets/6336b051-8211-4897-aeaf-4a169540a6f3" alt="Profil Bilgilerim Sayfası" width="600"/>
---
- **Departman Ekleme Ekranı(İK Uzmanı için):** Yeni departman ekleme ekranı.

  <img src="https://github.com/user-attachments/assets/131c07cc-6bc8-46d4-b425-3f223caddf39" alt="Görev Detayları Sayfası" width="600"/>
---
- **Personel Ekleme Ekranı(İK Uzmanı için):** Yeni personel ekleme ekranı.

  <img src="https://github.com/user-attachments/assets/5130ad33-a10d-4b32-897e-b85dfed510cf" alt="Yeni Görev Ekleme Sayfası" width="600"/>
---
- **Personel Listesi Ekranı(İK Uzmanı için):** Personellerin gösterildiği ekran.

  <img src="https://github.com/user-attachments/assets/5406c249-eaa6-4b34-82a7-f77ff183fe75" alt="Görev Güncelleme Sayfası" width="600"/>
---
- **Personel Silme Uyarısı(İK Uzmanı için):** Personel silmek istediğimde karşımıza çıkan uyarı pop-up penceresi.

  <img src="https://github.com/user-attachments/assets/b848efae-1fef-4d7f-a79d-68c25af1f4c6" alt="Departman Ekleme Sayfası" width="600"/>
---
- **Görev Takip Listesi:** Görevleri ve bu görevlere ait durumları görüp, görevlerle ilgili işlemlere ulaşabileceğimiz ekran.

  <img src="https://github.com/user-attachments/assets/c7b6f369-184a-4e0d-b0ac-9c819001a92f" alt="Personel Ekleme Sayfası" width="600"/>
---
- **Görev Ekleme Ekranı:** Bir departman deçip o departmana ait personelleri listeledikten sonra istediğimiz personele görevini atayabildiğimiz ekran.

  <img src="https://github.com/user-attachments/assets/a53b66bf-e2d2-454a-afa0-614c7afbb81f" alt="Personel Listesi Ekranı" width="600"/>
---
- **Görevlerim Ekranı:** Personelin kendisine atanmış görevleri görebildiği ve bu görevi onayalama, reddetetme seçeneklerinin yer aldığı ekran. (personel görevi kendisine atandıktan sonra 3 gün içinde onaylamaz ise görev `süresi doldu` olarak ayarkanmaktadır.)

  <img src="https://github.com/user-attachments/assets/82322b9f-52f2-482a-b1b3-771b4f44559a" alt="Personel Listesi Ekranı" width="600"/>
---
- **Görev Detayları Ekranı:** Görevin detaylarını görüntüleyip görevle ilgili silme ve güncelleme işlemlerine erişebildiğimiz ekran. (Burada ilgili görevi güncellemek veya silebilmek için bu görevi atayan kişi olmalısınız.)

  <img src="https://github.com/user-attachments/assets/4677fccc-e20f-4531-a9c2-83b52420c9ec" alt="Personel Ekleme Sayfası" width="600"/>
---
- **Görev Güncelleme Ekranı:** İlgili görevi güncelleyebildiğimiz ekran.

  <img src="https://github.com/user-attachments/assets/19c4a08a-d37c-4cdc-a203-18ea35557962" alt="Personel Ekleme Sayfası" width="600"/>
---
- **Görev Güncelleme Uyarısı:** Görevi güncellemek için butona bastığımızda çıkan pop-up penceresi.

  <img src="https://github.com/user-attachments/assets/9dd74f6b-347e-4672-aee5-a16abcd65569" alt="Profil Bilgilerim Sayfası"/>
---
- **Görev Silme Uyarısı:** Görevi silmek için butona bastığımızda çıkan pop-up penceresi.

  <img src="https://github.com/user-attachments/assets/7f2b4646-dece-4cb6-8d99-f0eb716f82fc" alt="Profil Bilgilerim Sayfası"/>
---
- **Profil Ekranı:** Kullanıcının kişisel bilgilerini görüp güncellemek isterse oraya yönlendirilecek butonun yer aldığı ekran. (Burada uygulama navbarda bulununan buton vasıtasıyla yabancı dile çevirlmiştir.)
- 
  <img src="https://github.com/user-attachments/assets/5851ef8c-fd4e-4f65-8a13-a5fa3c69f706" alt="Profil Bilgilerim Sayfası"/>
---
- **Profil Güncelleme Ekranı:** Kullanıcının kişisel bilgilerini güncelleyebildiği ekran. (Burada uygulama navbarda bulununan buton vasıtasıyla yabancı dile çevirlmiştir.)

  <img src="https://github.com/user-attachments/assets/0789112c-62b7-4580-a2ba-55b6830591e2" alt="Profil Bilgilerim Sayfası"/>
---
- **Access Denied Sayfası:** Erişim izni olmayan kullanıcılar için özel erişim engellendi sayfası.

  <img src="https://github.com/user-attachments/assets/a7e0cdb4-7f88-455c-8d62-babb2e6c8fcd" alt="Access Denied Sayfası" width="600"/>
---
- **Not Found Sayfası:** Mevcut olmayan bir sayfa talebi durumunda gösterilen sayfa.

  <img src="https://github.com/user-attachments/assets/2c9744d8-d4b3-4641-bc06-7c813ac4b37c" alt="Not Found Sayfası" width="600"/>

---

## Katkıda Bulunma

Projeyi fork ederek katkıda bulunabilirsiniz. Lütfen büyük değişiklikler için önce neyi değiştirmek istediğinizi tartışmak üzere bir konu açın.

---

## İletişim

- **Proje Sahibi:** Cengizhan Yıldız
- **E-posta:** cengizhanyildiz.14@outlook.com
