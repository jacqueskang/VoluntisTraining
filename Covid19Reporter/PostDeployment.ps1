# TODO: replace with correct value
$SubscriptionName = "Microsoft Azure/ Development"
$ResourceGroupName = "rg-azurelearning-jkang"
$ResourceGroupHash = "zeb3g5gllfxvw"

Set-AzContext -Subscription $SubscriptionName
$Subscription = Get-AzSubscription -SubscriptionName $SubscriptionName

# Create storage queues if necessary
$StorageAccountName = "stcovid19$ResourceGroupHash"
$StorageAccount = Get-AzStorageAccount -ResourceGroupName $ResourceGroupName -Name $StorageAccountName
$QueueNames = 
	"queue-new-reports",
	"queue-email-notifications",
	"queue-new-records"
foreach ($QueueName in $QueueNames) {
	$StorageQueue = Get-AzStorageQueue -Context $StorageAccount.Context -Name $QueueName -ErrorAction Ignore
	if ($null -eq $StorageQueue) {
		$StorageQueue = New-AzStorageQueue -Context $StorageAccount.Context -Name $QueueName
		Write-Host "Created storage queue '$QueueName'."
	}
	else {
		Write-Host "Storage queue '$QueueName' is already created. Skipping..."
	}
}

# Create event grid subscriptions if necessary
$Subscriptions = 
@{
	EventType = "ReportSubmitted"
	QueueName = "queue-new-reports"
},
@{
	EventType = "ReportAnalyzed"
	QueueName = "queue-email-notifications"
},
@{
	EventType = "ReportAnalyzed"
	QueueName = "queue-new-records"
}
$TopicName = "topic-covid19-$ResourceGroupHash"
foreach ($Item in $Subscriptions) {
	$EventSubscriptionName = $Item.QueueName
	$EventSubscription = Get-AzEventGridSubscription `
		-ResourceGroupName $ResourceGroupName `
		-TopicName $TopicName `
		-EventSubscriptionName $EventSubscriptionName `
		-ErrorAction Ignore
	if ($null -eq $EventSubscription) {
		$EventSubscription = New-AzEventGridSubscription `
			-ResourceGroupName $ResourceGroupName `
			-TopicName $TopicName `
			-EventSubscriptionName $EventSubscriptionName `
			-EndpointType storagequeue `
			-Endpoint "/subscriptions/$($Subscription.Id)/resourceGroups/$ResourceGroupName/providers/Microsoft.Storage/storageAccounts/$StorageAccountName/queueServices/default/queues/$($Item.QueueName)" `
			-IncludedEventType $Item.EventType
		Write-Host "Created event subscription '$EventSubscriptionName'."
	}
	else {
		Write-Host "Event subscription '$EventSubscriptionName' is already created. Skipping..."
	}
}
