# Görev Yönetim Sistemi

Bu proje, **Görev Yönetim Sistemi** için bir örnek uygulama içermektedir. Bu sistem, görev yönetimi süreçlerini dijitalleştirmeyi ve kolaylaştırmayı amaçlar. Kullanıcılar, görev atamaları yapabilir, görevleri takip edebilir ve görev durumlarını güncelleyebilir.

## Proje Hakkında

**Görev Yönetim Sistemi** projesi, görev yönetimi süreçlerini düzenlemek için tasarlanmış bir web uygulamasıdır. Kullanıcılar, görevleri oluşturabilir, atayabilir, güncelleyebilir ve silebilir. Sistem, kullanıcıların görev durumlarını takip etmelerini ve ilgili filtreler yardımıyla görevleri daha kolay yönetmelerini sağlar.

Proje, **RESTful API** mimarisi kullanılarak geliştirilmiştir. **JWT (JSON Web Token)** tabanlı kimlik doğrulama sistemi sayesinde, kullanıcı oturumları güvenli bir şekilde yönetilmektedir. **Code First** yaklaşımı ile veritabanı modelleri oluşturulmuş ve Entity Framework Core kullanılarak yönetilmiştir.

## Özellikler

- **Görev Yönetimi:** Kullanıcılar görev oluşturabilir, atayabilir ve yönetebilir.
- **Filtreleme:** Kullanıcılar, görev listesi üzerinde çeşitli filtreler kullanarak belirli görevleri arayabilir.
- **Görev Durumu:** Görevlerin durumu (Beklemede, Tamamlandı, Reddedildi) takip edilebilir.
- **Görev Onaylama/Reddetme:** Kullanıcılar kendilerine atanan görevleri onaylanabilir veya reddedilebilir.
- **Kullanıcı Profili:** Kullanıcılar profil bilgilerini görüntüleyebilir.
- **Oturum Yönetimi:**
  - **Beni Hatırla:** Giriş ekranında bulunan "Beni Hatırla" butonu ile kullanıcıların oturumları 1 gün boyunca açık kalabilir.
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

## Gereksinimler

- .NET 6.0
- ASP.NET Core MVC
- Entity Framework Core
- SQL Server
- Visual Studio veya Visual Studio Code
- Bootstrap
- JavaScript
- FontAwesome

## Data Katmanı

**Data** katmanı, projenin temel veri modellerini ve bu modellerin birbirleriyle olan ilişkilerini içerir. Bu katmanda yer alan Entity sınıfları, veritabanı tablolarını temsil eder ve Entity Framework Core tarafından veritabanı işlemleri için kullanılır.

### TaskManagerContext Sınıfı

TaskManagerContext sınıfı, Entity Framework Core kullanılarak veritabanı işlemlerinin yönetildiği temel sınıftır. Bu sınıf, veritabanı bağlantısı ve veritabanı üzerinde gerçekleştirilecek işlemler için gerekli olan yapılandırmaları içerir.

    public TaskManagerContext(DbContextOptions<TaskManagerContext> options) : base(options)
    {
    }

    // Veritabanında yer alacak tablolar
    public DbSet<User> Users { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<ToDoTask> Task { get; set; }

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

`TaskManagerContext` sınıfı ile tanımlanan veritabanı tablolarının Entity sınıfları ve aralarındaki ilişkiler yukarıda belirtilmiştir. Bu sınıf, Entity Framework Core kullanılarak oluşturulmuş olup, veritabanı işlemlerinin ve tablolar arası ilişkilerin yönetimini sağlar. 

Bu yapıda, kullanıcılar ve görevler arasındaki ilişkiler ile departmanlar ve görevler arasındaki ilişkiler doğru ve güvenli bir şekilde yönetilmiştir. Özellikle `DeleteBehavior.Restrict` kullanılarak, bağlı olan verilerin kazara silinmesini engelleyecek şekilde yapılandırma yapılmıştır.
    
### Entity Sınıfları ve İlişkileri

#### 1. **User (Kullanıcı) Sınıfı**

- **Properties:**
  - `Id`: Kullanıcının benzersiz kimlik numarası.
  - `Name`: Kullanıcının adı.
  - `LastName`: Kullanıcının soyadı.
  - `Email`: Kullanıcının e-posta adresi.
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

**İlişkiler:**
- `ToDoTask` sınıfı ile `Department` sınıfı arasında bire çok (one-to-many) bir ilişki vardır. Her görev, belirli bir departmana aittir.
- `ToDoTask` sınıfı ile `User` sınıfı arasında iki bire çok (one-to-many) ilişki vardır:
  - `CreaterUser`: Her görev bir kullanıcı tarafından oluşturulur.
  - `AsaignedUser`: Her görev bir kullanıcıya atanabilir.
- `ToDoTask` sınıfı ile `TaskStatusEnum` arasında bir enum ilişkisi vardır. Bu enum, görevlerin durumunu belirtir.

### Entity Sınıflarının Genel İlişkileri

- **Department** ve **User** sınıfları arasında bir departmanın birden fazla kullanıcıya sahip olabileceği, ancak her kullanıcının yalnızca bir departmana ait olabileceği bire çok (one-to-many) ilişkisi bulunmaktadır.
- **User** ve **ToDoTask** sınıfları arasında bir kullanıcının birden fazla görev oluşturabileceği ve birden fazla göreve atanabileceği iki ayrı bire çok (one-to-many) ilişkisi bulunmaktadır.
- **Department** ve **ToDoTask** sınıfları arasında bir departmanın birden fazla göreve sahip olabileceği bire çok (one-to-many) ilişkisi vardır.

Bu yapı, **Görev Yönetim Sistemi**'nde kullanıcıların ve departmanların görevleri nasıl oluşturup yönetebileceğini belirleyen temel veri modelini oluşturur. Bu sayede, sistemdeki görevler ve bu görevlerin hangi kullanıcılar tarafından oluşturulup, hangi kullanıcılara atandığı veritabanında doğru ve verimli bir şekilde saklanabilir.

## Kurulum

1. Projeyi klonlayın:
sh
    git clone https://github.com/Cengizhanyildiz14/TaskManagementSystem.git
    
2. Proje dizinine gidin:
sh
    cd TaskManagementSystem
    
3. Gerekli bağımlılıkları yükleyin:
sh
    dotnet restore
    
4. **Appsettings.json Konfigürasyonu:**

   Projeyi kullanmadan önce `appsettings.json` dosyasındaki bağlantı ayarlarını ve gizli anahtarları kendi sisteminize göre düzenlemeniz gerekmektedir. Örneğin:
json
    "ConnectionStrings": {
      "default": "server=YOUR_SERVER_NAME; Database=YOUR_DATABASE_NAME; integrated security=true; encrypt=false"
    },
    "ApiSettings": {
      "Secret": "YOUR_SECRET_KEY"
    }
    
- **ConnectionStrings:** `server`, `Database`, ve diğer bağlantı ayarlarını kendi sisteminize göre düzenleyin.
   - **ApiSettings:** `Secret` anahtarını, güvenliğinizi sağlamak için kendi özel ve gizli bir anahtarla değiştirin.

5. Veritabanını oluşturun:
sh
    dotnet ef database update
    
6. Uygulamayı çalıştırın:
sh
    dotnet run
    
## Kullanım

- **Giriş Yapma:** Kullanıcılar e-posta adresleri ile sisteme giriş yapabilir.
- **Beni Hatırla:** Giriş sırasında "Beni Hatırla" butonunu seçerek, oturumunuzu 1 gün boyunca açık tutabilirsiniz.
- **Görev Oluşturma:** Ana sayfadan yeni bir görev oluşturabilir ve ilgili kullanıcılara atayabilirsiniz.
- **Görev Güncelleme ve Silme:** Kendi oluşturduğunuz görevleri güncelleyebilir veya silebilirsiniz.
- **Görev Onaylama/Reddetme:** Atanan görevleri onaylayabilir veya reddedebilirsiniz.
- **Profil Yönetimi:** Kullanıcılar profil bilgilerini görüntüleyebilir.
- **Yetkilendirme:** İnsan Kaynakları Uzmanı olan kişiler departman ve kullanıcı yönetimi işlemleri yapabilir.
- **Yönlendirme:** Yetkiniz olmayan sayfalara erişmeye çalıştığınızda veya mevcut olmayan bir sayfaya yönlendirildiğinizde, sistem sizi ilgili özel sayfalara yönlendirir.

## Ekran Görüntüleri

- **Giriş Sayfası:** Kullanıcıların sisteme giriş yaptığı ekran.

  <img src="https://github.com/user-attachments/assets/19afe322-193f-44c7-9f50-05ee77613520" alt="Giriş Sayfası" width="600"/>

- **Anasayfa:** Görev yönetim işlemlerinin yapıldığı ana ekran.

  <img src="https://github.com/user-attachments/assets/8d2941c0-5787-43f4-b316-9a3ba2f6fe76" alt="Anasayfa" width="600"/>

- **Görevlerim Sayfası:** Kullanıcıya ait görevlerin listelendiği ekran.

  <img src="https://github.com/user-attachments/assets/e6ff8d73-7679-4dbb-8e93-2aafc9755bca" alt="Görevlerim Sayfası" width="600"/>

- **Profil Bilgilerim Sayfası:** Kullanıcının profil bilgilerini görüntüleyebildiği ekran.

  <img src="https://github.com/user-attachments/assets/791a95a3-8b27-4ec8-b7d4-647a34e42051" alt="Profil Bilgilerim Sayfası" width="600" style="display:block; margin:auto; background-color:#fff;"/>

- **Görev Detayları Sayfası:** Bir görevin detaylarının görüntülendiği ekran.

  <img src="https://github.com/user-attachments/assets/94ce2230-cb44-48b1-aeb7-96a5b8568e3c" alt="Görev Detayları Sayfası" width="600"/>

- **Yeni Görev Ekleme Sayfası:** Yeni bir görev oluşturma ekranı.

  <img src="https://github.com/user-attachments/assets/354da73a-cc6f-41c1-b2b3-1cd0f61a186e" alt="Yeni Görev Ekleme Sayfası" width="600"/>

- **Görev Güncelleme Sayfası:** Mevcut bir görevin güncellenebildiği ekran.

  <img src="https://github.com/user-attachments/assets/18a7d0c7-1fcd-48cb-aa6a-d9a2d10d211d" alt="Görev Güncelleme Sayfası" width="600"/>

- **Departman Ekleme Sayfası:** (İK Uzmanı için) Yeni departman ekleme ekranı.

  <img src="https://github.com/user-attachments/assets/9689b86e-aa20-4e5e-bb7a-7d91903a7bc4" alt="Departman Ekleme Sayfası" width="600"/>

- **Personel Ekleme Sayfası:** (İK Uzmanı için) Yeni personel ekleme ekranı.

  <img src="https://github.com/user-attachments/assets/dee63a88-5a37-47de-8d26-6fdea0c176f8" alt="Personel Ekleme Sayfası" width="600"/>

- **Personel Listesi Ekranı:** (İK Uzmanı için) Sistemdeki personelin listelendiği ekran.

  <img src="https://github.com/user-attachments/assets/bad93264-54a2-4cc0-8c09-ce8b9e838872" alt="Personel Listesi Ekranı" width="600"/>

- **Access Denied Sayfası:** Erişim izni olmayan kullanıcılar için özel erişim engellendi sayfası.

  <img src="https://github.com/user-attachments/assets/a7e0cdb4-7f88-455c-8d62-babb2e6c8fcd" alt="Access Denied Sayfası" width="600"/>

- **Not Found Sayfası:** Mevcut olmayan bir sayfa talebi durumunda gösterilen sayfa.

  <img src="https://github.com/user-attachments/assets/2c9744d8-d4b3-4641-bc06-7c813ac4b37c" alt="Not Found Sayfası" width="600"/>

## Katkıda Bulunma

Katkılarınızı bekliyoruz! Projeyi fork ederek katkıda bulunabilirsiniz. Lütfen büyük değişiklikler için önce neyi değiştirmek istediğinizi tartışmak üzere bir konu açın.

## İletişim

- **Proje Sahibi:** Cengizhan Yıldız
- **E-posta:** cengizhanyildiz.14@outlook.com
