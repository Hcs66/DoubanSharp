
using System;
using DoubanSharp.Model;
using RestSharp;
namespace DoubanSharp.Service
{
public partial class DoubanService
{
		public void GetBook(string subjectId, Action<DoubanBook, DoubanResponse> action)
		{
			ExecuteRequest(action, "v2/book/{subjectId}", "?subjectId=", subjectId);
		}

		public void GetBookByISBN(string isbn, Action<DoubanBook, DoubanResponse> action)
		{
			ExecuteRequest(action, "v2/book/isbn/{isbn}", "?isbn=", isbn);
		}

		public void GetMovie(string subjectId, Action<DoubanMovie, DoubanResponse> action)
		{
			ExecuteRequest(action, "v2/movie/{subjectId}", "?subjectId=", subjectId);
		}

		public void GetMusic(string subjectId, Action<DoubanMusic, DoubanResponse> action)
		{
			ExecuteRequest(action, "v2/music/{subjectId}", "?subjectId=", subjectId);
		}

		public void SearchBooks(string q, string tag, string start, string count, Action<DoubanBookSearch, DoubanResponse> action)
		{
			ExecuteRequest(action, "v2/book/search", "?q=", q, "&tag=", tag, "&start=", start, "&count=", count);
		}

		public void SearchMovies(string q, string tag, string start, string count, Action<DoubanMovieSearch, DoubanResponse> action)
		{
			ExecuteRequest(action, "v2/movie/search", "?q=", q, "&tag=", tag, "&start=", start, "&count=", count);
		}

		public void SearchMusics(string q, string tag, string start, string count, Action<DoubanMusicSearch, DoubanResponse> action)
		{
			ExecuteRequest(action, "v2/music/search", "?q=", q, "&tag=", tag, "&start=", start, "&count=", count);
		}

		public void SearchMiniBlogs(string start, string count, Action<DoubanMiniBlogSearch, DoubanResponse> action)
		{
			ExecuteRequest(action, "shuo/v2/statuses/home_timeline", "?start=", start, "&count=", count);
		}

       public void AddMiniBlog(DoubanMiniBlog miniBlog,string text, Action<DoubanResponse> action)
		{
            ExecuteRequest(Method.POST, action, "shuo/v2/statuses/",miniBlog, "?text=", text);
		}
			
		public void GetPeople(string userId, Action<DoubanPeople, DoubanResponse> action)
		{
			ExecuteRequest(action, "v2/user/{userId}", "?userId=", userId);
		}

		public void GetPeopleAuthorized(Action<DoubanPeople, DoubanResponse> action)
		{
			ExecuteRequest(action, "v2/user/~me");
		}

		public void SearchPeople(string q, string start, string count, Action<DoubanPeopleSearch, DoubanResponse> action)
		{
			ExecuteRequest(action, "v2/user", "?q=", q, "&start=", start, "&count=", count);
		}

}
}
