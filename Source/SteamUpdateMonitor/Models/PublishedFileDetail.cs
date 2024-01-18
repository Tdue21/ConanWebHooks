namespace SteamUpdateMonitor.Models;

public class PublishedFileDetail
{
    public int result { get; set; }
    public string publishedfileid { get; set; }
    public string creator { get; set; }
    public int creator_appid { get; set; }
    public int consumer_appid { get; set; }
    public int consumer_shortcutid { get; set; }
    public string filename { get; set; }
    public string file_size { get; set; }
    public string preview_file_size { get; set; }
    public string preview_url { get; set; }
    public string url { get; set; }
    public string hcontent_file { get; set; }
    public string hcontent_preview { get; set; }
    public string title { get; set; }
    public string short_description { get; set; }
    public int time_created { get; set; }
    public int time_updated { get; set; }
    public int visibility { get; set; }
    public int flags { get; set; }
    public bool workshop_file { get; set; }
    public bool workshop_accepted { get; set; }
    public bool show_subscribe_all { get; set; }
    public int num_comments_public { get; set; }
    public bool banned { get; set; }
    public string ban_reason { get; set; }
    public string banner { get; set; }
    public bool can_be_deleted { get; set; }
    public string app_name { get; set; }
    public int file_type { get; set; }
    public bool can_subscribe { get; set; }
    public int subscriptions { get; set; }
    public int favorited { get; set; }
    public int followers { get; set; }
    public int lifetime_subscriptions { get; set; }
    public int lifetime_favorited { get; set; }
    public int lifetime_followers { get; set; }
    public string lifetime_playtime { get; set; }
    public string lifetime_playtime_sessions { get; set; }
    public int views { get; set; }
    public int num_children { get; set; }
    public int num_reports { get; set; }
    public int language { get; set; }
    public bool maybe_inappropriate_sex { get; set; }
    public bool maybe_inappropriate_violence { get; set; }
    public string revision_change_number { get; set; }
    public int revision { get; set; }
    public int ban_text_check_result { get; set; }
}


public class SimplePublishedFileDetail
{
    public string publishedfileid { get; set; }
    public int result { get; set; }
    public string creator { get; set; }
    public int creator_app_id { get; set; }
    public int consumer_app_id { get; set; }
    public string filename { get; set; }
    public int file_size { get; set; }
    public string file_url { get; set; }
    public string hcontent_file { get; set; }
    public string preview_url { get; set; }
    public string hcontent_preview { get; set; }
    public string title { get; set; }
    public string description { get; set; }
    public int time_created { get; set; }
    public int time_updated { get; set; }
    public int visibility { get; set; }
    public int banned { get; set; }
    public string ban_reason { get; set; }
    public int subscriptions { get; set; }
    public int favorited { get; set; }
    public int lifetime_subscriptions { get; set; }
    public int lifetime_favorited { get; set; }
    public int views { get; set; }
    public object[] tags { get; set; }
}
