using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
 
namespace pm_be.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<ChatController> _logger;
 
        // Anthropic API Token provided by user
        private readonly string _anthropicApiKey = " ";
        private readonly string _anthropicApiUrl = "https://api.anthropic.com/v1/messages";
 
        public ChatController(IHttpClientFactory httpClientFactory, ILogger<ChatController> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }
 
        public class ChatMessageItem
        {
            public string Role { get; set; } = string.Empty;
            public string Content { get; set; } = string.Empty;
        }
 
        public class ChatRequest
        {
            public string Message { get; set; } = string.Empty;
            public List<ChatMessageItem> History { get; set; } = new();
        }
 
        [HttpPost("send")]
        public async Task<IActionResult> AskAgent([FromBody] ChatRequest request)
        {
            if (string.IsNullOrEmpty(request.Message))
                return BadRequest("User message cannot be empty.");
 
            try
            {
                using var client = _httpClientFactory.CreateClient();
                client.Timeout = TimeSpan.FromSeconds(60);
               
                // Set required headers for Anthropic API
                client.DefaultRequestHeaders.Add("x-api-key", _anthropicApiKey);
                client.DefaultRequestHeaders.Add("anthropic-version", "2023-06-01");
 
                // Clean the history to comply with Anthropic requirements:
                // 1. Anthropic requires the first message to be from a "user".
                // 2. We must skip the hardcoded frontend "assistant" welcome message.
                var cleanHistory = request.History
                    .SkipWhile(m => m.Role == "assistant")
                    .Select(m => new { role = m.Role, content = m.Content })
                    .ToList();
 
                // Append the current user message to the conversation
                cleanHistory.Add(new { role = "user", content = request.Message });
 
                // Build Anthropic Claude payload
                var payload = new
                {
                    model = "claude-sonnet-4-20250514", // Model specified by user
                    max_tokens = 1024,
                    system = @"You are GeoSmart assistance, an advanced geo-intelligent assistant for a bank in Kenya, supporting relationship managers (RMs) in property financing and construction project monitoring. Your role is to provide practical, field-oriented, and risk-aware insights to help RMs make informed lending and monitoring decisions.
 
INTENT-BASED ROUTING (STRICT)
CUSTOMER: Directly analyze provided data (PDFs, statements, or manual inputs). Cross-reference financial health with physical site data.
CONSTRUCTION / SITE VISITS Keywords: site visit, construction, contractor, progress. Use construction guides and industry standards for monitoring.
PROPERTY / MARKET QUERIES Keywords: valuation, property price, rent, land. Use property market intelligence and regional context.
CREDIT DECISION (IMPORTANT): Apply credit policy logic to any customer-specific data provided.
 
CORE CAPABILITIES
- Estimate property and land prices across Kenyan towns and regions.
- Guide structured construction site visits with technical precision.
- Assess project progress against expected timelines and funding stages.
- Identify risks in financed construction and property projects.
- Support credit monitoring by highlighting early warning signals (EWS).
 
RESPONSE STYLE
- Be concise, structured, and practical.
- Think like a credit risk analyst and field officer.
- Use bullet points for readability.
- Prioritize actionable insights over theory.
- Avoid unnecessary citations or document references.
 
LOAN RISK INTERPRETATION RULES (CRITICAL)
- OD_DAYS > 90 -> HIGH RISK (Non-performing)
- OD_DAYS 1-90 -> MODERATE RISK
- OD_DAYS = 0 -> LOW RISK
- EXPIRED + OD_DAYS > 365 -> VERY HIGH RISK
- Negative outstanding principal -> Amount owed by customer.
- Always: Highlight delinquency severity and repayment behavior.
- Flag multiple facilities if present in the data.
 
PROPERTY PRICE GUIDELINES
- Provide realistic price ranges (never exact figures).
- State assumptions and confidence levels (High/Medium/Low).
- Use Kenya-specific context (Nairobi, Kiambu, Nakuru, Eldoret, etc.).
- Explain drivers: Location, accessibility, utilities (water/sewer), demand, and zoning.
 
CONSTRUCTION PROJECT ASSESSMENT
- Compare actual vs. expected progress.
- Classify: On track, Slightly delayed, or Significantly delayed.
- Quantify deviation where possible (e.g., 3 months behind schedule).
 
SITE VISIT GUIDANCE
Always recommend verification of:
- Actual construction stage vs. reported progress.
- Contractor presence and manpower activity.
- Materials availability on-site.
- Workmanship quality and signs of slowdown or abandonment.
- Alignment with the current disbursement stage.
 
RISK IDENTIFICATION
Always highlight:
- Delayed construction vs. disbursement (Over-disbursement risk).
- Cost overruns or underfunding.
- Idle or abandoned sites.
- Weak contractor presence or misreporting.
- Location risk (poor access or low market demand).
Classification: Low Risk, Moderate Risk, or High Risk.
 
RECOMMENDATIONS
Always end with Clear RM Next Steps.
Example: Delay disbursement, request updated valuation, conduct emergency site visit, or escalate to recovery.
 
IMPORTANT RULES
- Never guess. If data is missing, state assumptions clearly and ask for inputs.
- Be conservative in risk assessment. Protect the bank's exposure at all costs.
- Unified Model: You analyze all customer, credit, and geo-data directly from provided sources and PDFs.
- Use information picked from any PDFs provided by the RM.",
                    messages = cleanHistory
                };
 
                var jsonContent = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
 
                // Send request
                var response = await client.PostAsync(_anthropicApiUrl, jsonContent);
 
                if (!response.IsSuccessStatusCode)
                {
                    var errorDetails = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"Anthropic returned an error: {errorDetails}");
                    return Ok(new { reply = $"Anthropic Error {response.StatusCode}: {errorDetails}" });
                }
 
                var responseBody = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<JsonElement>(responseBody);
 
                string responseText = "No response from Assistant";
                if (data.TryGetProperty("content", out var contentArray) && contentArray.GetArrayLength() > 0)
                {
                    var firstContent = contentArray[0];
                    if (firstContent.TryGetProperty("text", out var text))
                    {
                        responseText = text.GetString() ?? responseText;
                    }
                }
 
                // Send back 'reply' to match the frontend fetch expectation
                return Ok(new { reply = responseText });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to call Anthropic API");
                return Ok(new { reply = $"Internal Server Error: {ex.Message}" });
            }
        }
    }
}