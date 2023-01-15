namespace VOD.Membership.Database.Extensions;

public static class VODContextExtensions
{
    public static async Task SeedMembershipData(this IDbService service)
    {
        #region Lorem Ipsum - Dummy Data
        var description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.";
        #endregion

        try
        {
            #region Add Instructors
            await service.AddAsync<Instructor, InstructorDTO>(new InstructorDTO
            {
                Name = "John Doe",
                Description = description.Substring(20, 50),
                Avatar = "/images/Ice-Age-Scrat-icon.png"
            });

            await service.AddAsync<Instructor, InstructorDTO>(new InstructorDTO
            {
                Name = "Jane Doe",
                Description = description.Substring(30, 40),
                Avatar = "/images/Ice-Age-Scrat-icon.png"
            });

            await service.SaveChangesAsync();
            #endregion

            #region Add Courses
            var instructor1 = await service.SingleAsync<Instructor, InstructorDTO>(c => c.Name.Equals("John Doe"));
            var instructor2 = await service.SingleAsync<Instructor, InstructorDTO>(c => c.Name.Equals("Jane Doe"));
            await service.AddAsync<Course, CourseDTO>(new CourseDTO
            {
                InstructorId = instructor1.Id,
                Title = "Course 1",
                Description = description,
                ImageUrl = "/images/course1.jpg",
                MarqueeImageUrl = "/images/laptop.jpg"
            });
            await service.AddAsync<Course, CourseDTO>(new CourseDTO
            {
                InstructorId = instructor2.Id,
                Title = "Course 2",
                Description = description,
                ImageUrl = "/images/course2.jpg",
                MarqueeImageUrl = "/images/laptop.jpg",
                Free = true
            });
            await service.AddAsync<Course, CourseDTO>(new CourseDTO
            {
                InstructorId = instructor1.Id,
                Title = "Course 3",
                Description = description,
                ImageUrl = "/images/course3.jpg",
                MarqueeImageUrl = "/images/laptop.jpg"
            });

            await service.SaveChangesAsync();
            #endregion

            #region Add Modules if they don't already exist
            var course1 = await service.SingleAsync<Course, CourseDTO>(c => c.Title.Equals("Course 1"));
            var course2 = await service.SingleAsync<Course, CourseDTO>(c => c.Title.Equals("Course 2"));
            var course3 = await service.SingleAsync<Course, CourseDTO>(c => c.Title.Equals("Course 3"));

            await service.AddAsync<Section, SectionDTO>(new SectionDTO { CourseId = course1.Id, Title = "Section 1" });
            await service.AddAsync<Section, SectionDTO>(new SectionDTO { CourseId = course1.Id, Title = "Section 2" });
            await service.AddAsync<Section, SectionDTO>(new SectionDTO { CourseId = course2.Id, Title = "Section 3" });

            await service.SaveChangesAsync();
            #endregion

            #region Add Videos if they don't already exist
            var section1 = await service.SingleAsync<Section, SectionDTO>(c => c.Title.Equals("Section 1"));
            var section2 = await service.SingleAsync<Section, SectionDTO>(c => c.Title.Equals("Section 2"));
            var section3 = await service.SingleAsync<Section, SectionDTO>(c => c.Title.Equals("Section 3"));

            await service.AddAsync<Video, VideoDTO>(new VideoDTO
            {
                SectionId = section1.Id,
                Title = "Video 1 Title",
                Description = description.Substring(1, 35),
                Duration = 50,
                Thumbnail = "/images/video1.jpg",
                Url = "https://www.youtube.com/embed/BJFyzpBcaCY"
            });
            await service.AddAsync<Video, VideoDTO>(new VideoDTO
            {
                SectionId = section1.Id,
                Title = "Video 2 Title",
                Description = description.Substring(5, 35),
                Duration = 45,
                Thumbnail = "/images/video2.jpg",
                Url = "https://www.youtube.com/embed/BJFyzpBcaCY"
            });
            await service.AddAsync<Video, VideoDTO>(new VideoDTO
            {
                SectionId = section1.Id,
                Title = "Video 3 Title",
                Description = description.Substring(10, 35),
                Duration = 41,
                Thumbnail = "/images/video3.jpg",
                Url = "https://www.youtube.com/embed/BJFyzpBcaCY"
            });
            await service.AddAsync<Video, VideoDTO>(new VideoDTO
            {
                SectionId = section3.Id,
                Title = "Video 4 Title",
                Description = description.Substring(15, 35),
                Duration = 41,
                Thumbnail = "/images/video4.jpg",
                Url = "https://www.youtube.com/embed/BJFyzpBcaCY"
            });
            await service.AddAsync<Video, VideoDTO>(new VideoDTO
            {
                SectionId = section2.Id,
                Title = "Video 5 Title",
                Description = description.Substring(20, 35),
                Duration = 42,
                Thumbnail = "/images/video5.jpg",
                Url = "https://www.youtube.com/embed/BJFyzpBcaCY"
            });

            await service.SaveChangesAsync();
            #endregion
        }
        catch(Exception ex)
        {
            throw;
        }
    }
}
