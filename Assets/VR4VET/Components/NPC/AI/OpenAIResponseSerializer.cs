using System;
using System.Collections.Generic;


// Field definitions for all of our serializable structures, for JSON requests and responses
[Serializable]
public class Message
{
	public string role;
	public string content;
	public object refusal;
}

[Serializable]
public class Choice
{
	public int index;
	public Message message;
	public object logprobs;
	public object finish_reason;
}

[Serializable]
public class CompletionTokenDetails
{
	public int reasoning_tokens;
}

[Serializable]
public class Usage
{
	public int prompt_tokens;
	public int completion_tokens;
	public int total_tokens;
	public CompletionTokenDetails completion_token_details;
}

[Serializable]
public class OpenAIResponse
{
	public string id;
	public string object_;
	public string created;
	public string model;
	public List<Choice> choices;
	public Usage usage;
	public string system_fingerprint;
}

[Serializable]
public class OpenAIRequest
{
    public string model;
    public List<Message> messages;
    public int max_tokens;
}