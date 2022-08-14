using AutoMapper;
using Neo4jClient;
using ShareBook.Application.Common.Contracts;
using ShareBook.Domain.Models.Media;
using ShareBook.Domain.Models.User;
using ShareBook.Infrastructure.Common.Extensions;
using ShareBook.Infrastructure.Common.Persistence.Mappings;
using ShareBook.Infrastructure.Common.Persistence.Repositories;

namespace ShareBook.Infrastructure.Images.Repositories;

public class GraphMediaRepository :
	AuditableGraphRepository<Media, Guid>,
	IMediaRepository
{
	private const string HasMediaRelation = "HAS_MEDIA";
	private const string HasProfileRelation = "HAS";
	
	public GraphMediaRepository(
		IBoltGraphClient graphClient,
		INodeMapping<Media> mapper,
		IDateTime dateTime)
		 : base(graphClient, mapper, dateTime) { }

	public async Task<Media> SaveProfileMediaAsync(UserId userId, Media media)
	{
		var query = _graphClient
			.Cypher
			.Match($"(u: {nameof(User)})-[:{HasProfileRelation}]->(p:{nameof(Profile)})")
			.Where((User u) => u.Id == userId)
			.With("u, p")
			.Merge($"(p)-[r: {HasMediaRelation}]->(m: {nameof(Media)})")
			.OnCreate()
			.Set($"m.{CreatedOn} = $now, m.{ModifiedOn} = $now, m.{Id} = apoc.create.uuid(), m += $media")
			.OnMatch()
			.Set($"m.{ModifiedOn} = $now, m += $media")
			.WithParams(new
			{
				media = _mapper.Map(media),
				now = _dateTime.Now
			})
			.Return<Media>("m");

		return await query.GetResult();
	}

	public Task<Media> GetByUserIdAsync(UserId userId)
		=> _graphClient
		          .Cypher
		          .Match($"(u: {nameof(User)})-[:{HasProfileRelation}]->(p: {nameof(Profile)})-[hm: {HasMediaRelation}]->(m: {nameof(Media)})")
		          .Where<User>(u => u.Id == userId)
		          .Return<Media>("m")
		          .GetResult();
}